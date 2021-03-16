using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace FullApp1.Core
{
    [MarkupExtensionReturnType(typeof(BindingExpression))]
    public class LocalizedLangExtension : MarkupExtension, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static bool? DesignMode = null;
        private static EventHandler<EventArgs> LocalLanguageChangeEvent;
        private static ResourceManager ResourceManager;
        private static CultureInfo CurrentCulture = new CultureInfo("en-US");

        public LocalizedLangExtension()
        {
            LocalLanguageChangeEvent += new EventHandler<EventArgs>(LocalLanguageChangedHandler);

            Init();
        }

        public LocalizedLangExtension(string key)
            : this()
        {
            Key = key;
        }

        public void Init()
        {
            // 本地化语言资源文件根名
            ResourceManager = new ResourceManager("FullApp1.Core.Resources.String.Resource", typeof(LocalizedLangExtension).Assembly);

            // 设置与系统相同的本地语言
            string lang = System.Globalization.CultureInfo.CurrentUICulture.Name;
            SetLocalLanguage(lang);
        }

        [ConstructorArgument("key")]
        public string Key { get; set; }

        private string mDefaultValue;
        /// <summary>
        /// 默认值，为了使在设计器的情况时把默认值绑到设计器
        /// </summary>
        public string DefaultValue
        {
            get => mDefaultValue;
            set => mDefaultValue = value;
        }

        private string mCurrentValue;
        /// <summary>
        /// 资源的具体内容，通过资源名称也就是上面的Key找到对应内容
        /// </summary>
        public string CurrentValue
        {
            /* get */
            get
            {
                /* 如果为设计器模式,本地的资源没有实例化，我们不能从资源文件得到内容，所以从KEY或默认值绑定到设计器去 */
                if (IsDesignMode)
                {
                    return DefaultValue ?? Key ?? "Error";
                }
                else
                {
                    if (null != Key)
                    {
                        return GetString(Key);
                    }
                }

                return mCurrentValue;
            }

            /* set */
            set => mCurrentValue = value;
        }

        /// <summary>
        /// 语言改变通知事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocalLanguageChangedHandler(object sender, EventArgs e)
        {
            // 通知CurrentValue值已经改变，需重新获取
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentValue)));
            }
        }

        /// <summary>
        /// 返回一个表达式对象，即绑定结果
        /// </summary>
        /// <param name="serviceProvider">serviceProvider是一个依赖解析器，你可以使用它来获取一个命名IProvideValueTarget的服务，然后会被用来获取属性的MarkupExtension （你可以得到它的目标对象）的目标。</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            Setter setter = target.TargetObject as Setter;
            if (setter != null)
            {
                string ss = setter.TargetName;
                return new Binding(nameof(CurrentValue)) { Source = this, Mode = BindingMode.OneWay };
            }
            else
            {
                Binding binding = new Binding(nameof(CurrentValue)) { Source = this, Mode = BindingMode.OneWay };
                return binding.ProvideValue(serviceProvider);
            }
        }

        /// <summary>
        /// 判断是设计器还是程序运行
        /// </summary>
        public static bool IsDesignMode
        {
            get
            {
                if (null == DesignMode)
                {
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    DesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;

                    if (!DesignMode.GetValueOrDefault(false)
                        && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
                    {
                        DesignMode = true;
                    }
                }

                return DesignMode.GetValueOrDefault(false);
            }
        }


        public static Action? ChangeLange;
        /// <summary>
        /// 修改本地语言
        /// </summary>
        /// <param name="language"></param>
        public static void SetLocalLanguage(string language)
        {
            CultureInfo culture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            CurrentCulture = culture;

            if (null != LocalLanguageChangeEvent)
            {
                ChangeLange?.Invoke();
                LocalLanguageChangeEvent(null, null);
            }
        }

        /// <summary>
        /// 根据KEY获取文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            string resault = null;

            try
            {
                resault = ResourceManager?.GetString(key, CurrentCulture);
            }
            finally
            {
                resault = resault ?? "Error";
            }
            return resault;
        }
    }

}
