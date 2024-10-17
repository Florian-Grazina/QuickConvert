namespace QuickConvert.FontAwesome
{
    public class FontAwesomeLabel : Button
    {
        public FontAwesomeLabel()
        {
            Populate();
        }

        public bool UseSolidFont
        {
            get { return (bool)GetValue(UseSolidFontProperty); }
            set { SetValue(UseSolidFontProperty, value); }
        }

        public static readonly BindableProperty UseSolidFontProperty =
            BindableProperty.Create(nameof(UseSolidFont), typeof(bool), typeof(FontAwesomeLabel),
                defaultValue: false,
                propertyChanged: (bindable, oldValue, newValue) =>
                {

                    ((FontAwesomeLabel)bindable).Populate();
                });

        private void Populate()
        {
            FontFamily = "SolidFA";
        }
    }
}
