namespace QuickConvert.FontAwesome
{
    public class FontAwesomeButton : Button
    {
        public FontAwesomeButton()
        {
            Populate();
        }

        public bool UseSolidFont
        {
            get { return (bool)GetValue(UseSolidFontProperty); }
            set { SetValue(UseSolidFontProperty, value); }
        }

        public static readonly BindableProperty UseSolidFontProperty =
            BindableProperty.Create(nameof(UseSolidFont), typeof(bool), typeof(FontAwesomeButton),
                defaultValue: false,
                propertyChanged: (bindable, oldValue, newValue) =>
                {

                    ((FontAwesomeButton)bindable).Populate();
                });

        private void Populate()
        {
            FontFamily = "SolidFA";
        }
    }
}
