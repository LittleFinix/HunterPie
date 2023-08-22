namespace HunterPie.UI.Architecture.Effects;

// public class AlligatorShaderEffect : ShaderEffect
// {
//     public Brush Input
//     {
//         get => (Brush(InputProperty);
//         set => SetValue(InputProperty, value);
//     }
//
//     public static readonly DependencyProperty InputProperty =
//         ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(AlligatorShaderEffect), 0);
//
//     public Brush RandomInput
//     {
//         get => (Brush(RandomInputProperty);
//         set => SetValue(RandomInputProperty, value);
//     }
//
//     public static readonly DependencyProperty RandomInputProperty =
//         ShaderEffect.RegisterPixelShaderSamplerProperty("RandomInput", typeof(AlligatorShaderEffect), 1);
//
//     public double Ratio
//     {
//         get => (double(RatioProperty);
//         set => SetValue(RatioProperty, value);
//     }
//
//     public static readonly DependencyProperty RatioProperty =
//         DependencyProperty.Register("Ratio", typeof(double), typeof(AlligatorShaderEffect), new UIPropertyMetadata(0.5, ShaderEffect.PixelShaderConstantCallback(0)));
//
//     public AlligatorShaderEffect()
//     {
//         // PixelShader = new PixelShader()
//         // {
//         //     UriSource = new Uri("avares:///HunterPie.UI;component/Architecture/Shaders/noise.ps")
//         // };
//
//         var bitmap = new BitmapImage();
//         bitmap.BeginInit();
//         bitmap.UriSource = new Uri("avares:///HunterPie.UI;component/Assets/Textures/alligator_noise_512x512.jpg");
//         bitmap.EndInit();
//
//         RandomInput = new ImageBrush(bitmap)
//         {
//             TileMode = TileMode.Tile,
//             Viewport = new Rect(0.0, 0.0, 1000, 1000.0),
//             ViewportUnits = BrushMappingMode.Absolute
//         };
//
//         UpdateShaderValue(InputProperty);
//         UpdateShaderValue(RandomInputProperty);
//         UpdateShaderValue(RatioProperty);
//     }
// }
