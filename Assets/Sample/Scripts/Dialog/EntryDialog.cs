using System;
using System.Collections.Generic;
using QAUI;
using UnityEngine;

public class EntryDialog : IDialog
{
    private readonly string _title;
    private readonly string _keyLabel;
    private readonly string _valueLabel;
    private readonly string _positiveButton;
    private readonly string _negativeButton;

    private readonly Action<string, string> _onPositiveButtonClicked;
    private readonly Action _onNegativeButtonClicked;

    private UIInputField _keyInputField;
    private UIInputField _valueInputField;

    public EntryDialog(
        string title = "Title",
        string keyLabel = "Key Label",
        string valueLabel = "Value Label",
        string positiveButton = "OK",
        string negativeButton = "CANCEL",
        Action<string, string> onPositiveButtonClicked = null,
        Action onNegativeButtonClicked = null
    )
    {
        _title = title;
        _keyLabel = keyLabel;
        _valueLabel = valueLabel;
        _positiveButton = positiveButton;
        _negativeButton = negativeButton;
        _onPositiveButtonClicked = onPositiveButtonClicked;
        _onNegativeButtonClicked = onNegativeButtonClicked;
    }

    public void Initialize(GameObject content)
    {
        UIText.Create(content)
            .SetFont(ResourceManager.GetFont(OpenSansFont.Bold))
            .SetText(_title);

        _keyInputField = UIInputField.Create(content)
            .SetLabel(it => { it.SetText(_keyLabel).UseContentHeight(); });

        _valueInputField = UIInputField.Create(content)
            .SetLabel(it => { it.SetText(_valueLabel).UseContentHeight(); });

        UISpacer.Create(content);
        UIHorizontalLayout.Create(content)
            .SetHeight(100)
            .SetSpacing(10)
            .AddElements(hl =>
            {
                return new List<UILayoutElement>()
                {
                    UILayoutElement.Create(
                            UIButton.Create(hl)
                                .SetText(it => { it.SetText(_negativeButton); })
                                .SetOnClickListener(_onNegativeButtonClicked)
                                .gameObject
                        )
                        .SetFlexibleWidth(1),

                    UILayoutElement.Create(
                            UIButton.Create(hl)
                                .SetText(it => { it.SetText(_positiveButton); })
                                .SetOnClickListener(() =>
                                {
                                    _onPositiveButtonClicked(_keyInputField.Text, _valueInputField.Text);
                                })
                                .gameObject
                        )
                        .SetFlexibleWidth(1)
                };
            });
    }
}