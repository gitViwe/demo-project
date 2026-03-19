namespace Blazor.Component.Layout;

public class HubDefaultTheme : MudTheme
{
    public static HubDefaultTheme Default => new();
    private HubDefaultTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = Colors.DeepPurple.Darken1,
            Background = "#d4e1e7",
            DrawerBackground = "#d4e1e7",
            DrawerText = "rgba(0,0,0, 0.7)",
            Surface = "#d4e1e7"
        };

        PaletteDark = new PaletteDark()
        {
            Primary = Colors.DeepPurple.Lighten1,
            DrawerBackground = "#32333d"
        };

        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "6px",
        };

        Typography = new MudBlazor.Typography()
        {
            Default = new DefaultTypography()
            {
                FontFamily = ["Poppins", "Montserrat", "Roboto", "sans-serif"],
            },
            
            Button = new DefaultTypography()
            {
                TextTransform = "none"
            },
        };
        
        Shadows = new Shadow();
        ZIndex = new ZIndex();

    }
}
