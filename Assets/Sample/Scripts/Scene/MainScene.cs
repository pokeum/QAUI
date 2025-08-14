using System.Globalization;
using QAUI;
using UnityEngine;

public class MainScene : Scene
{
    public override string Title() => "Main";

    public override void Initialize(GameObject content)
    {
        UIText.Create(content)
            .SetFont(ResourceManager.GetFont(OpenSansFont.Bold))
            .SetFontSize(45)
            .SetText("Navigation");

        UIButton.Create(content)
            .SetText(it => { it.SetText("Other Scene"); })
            .SetIcon()
            .SetOnClickListener(SceneNavigation.StartScene<OtherScene>);

        UISpacer.Create(content).SetFrame(30);

        UIText.Create(content)
            .SetFont(ResourceManager.GetFont(OpenSansFont.Bold))
            .SetFontSize(45)
            .SetText("Dialog");

        UIButton.Create(content)
            .SetText(it => { it.SetText("Show Value Dialog"); })
            .SetIcon()
            .SetOnClickListener(() =>
            {
                ShowDialog(new ValueDialog(
                    onPositiveButtonClicked: value =>
                    {
                        Debug.Log($"Value Dialog: value={{{value}}}");
                        CloseDialog();
                    },
                    onNegativeButtonClicked: CloseDialog
                ));
            });

        UIButton.Create(content)
            .SetText(it => { it.SetText("Show Entry Dialog"); })
            .SetIcon()
            .SetOnClickListener(() =>
            {
                ShowDialog(new EntryDialog(
                    onPositiveButtonClicked: (key, value) =>
                    {
                        Debug.Log($"Entry Dialog: key={{{key}}}, value={{{value}}}");
                        CloseDialog();
                    },
                    onNegativeButtonClicked: CloseDialog
                ));
            });

        UIButton.Create(content)
            .SetText(it => { it.SetText("Show Typed Entry Dialog"); })
            .SetIcon()
            .SetOnClickListener(() =>
            {
                ShowDialog(new TypedEntryDialog<DataType>(
                    defaultType: DataType.String,
                    onPositiveButtonClicked: (key, value, type) =>
                    {
                        switch (type)
                        {
                            case DataType.Int:
                                if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture,
                                        out var intValue))
                                    Debug.Log($"Typed Entry Dialog: key={{{key}}}, value={{{intValue}}}");
                                else
                                {
                                    Debug.LogError(
                                        $"Typed Entry Dialog: Failed to parse '{value}' as a valid Int value.");
                                    return;
                                }

                                break;
                            case DataType.Long:
                                if (long.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture,
                                        out var longValue))
                                    Debug.Log($"Typed Entry Dialog: key={{{key}}}, value={{{longValue}}}");
                                else
                                {
                                    Debug.LogError(
                                        $"Typed Entry Dialog: Failed to parse '{value}' as a valid Long value.");
                                    return;
                                }

                                break;
                            case DataType.Float:
                                if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture,
                                        out var floatValue))
                                    Debug.Log($"Typed Entry Dialog: key={{{key}}}, value={{{floatValue}}}");
                                else
                                {
                                    Debug.LogError(
                                        $"Typed Entry Dialog: Failed to parse '{value}' as a valid Float value.");
                                    return;
                                }

                                break;
                            case DataType.Double:
                                if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture,
                                        out var doubleValue))
                                    Debug.Log($"Typed Entry Dialog: key={{{key}}}, value={{{doubleValue}}}");
                                else
                                {
                                    Debug.LogError(
                                        $"Typed Entry Dialog: Failed to parse '{value}' as a valid Double value.");
                                    return;
                                }

                                break;
                            case DataType.Boolean:
                                if (bool.TryParse(value, out var boolValue))
                                    Debug.Log($"Typed Entry Dialog: key={{{key}}}, value={{{boolValue}}}");
                                else
                                {
                                    Debug.LogError(
                                        $"Typed Entry Dialog: Failed to parse '{value}' as a valid Boolean value.");
                                    return;
                                }

                                break;
                            case DataType.String:
                                Debug.Log($"Typed Entry Dialog: key={{{key}}}, value={{{value}}}");
                                break;
                        }

                        CloseDialog();
                    },
                    onNegativeButtonClicked: CloseDialog
                ));
            });
    }
}