using System;
using System.Collections.Generic;
using System.Linq;
using QAUI;
using UnityEngine;

public class TypedEntryDialog<T> : IDialog where T : Enum
{
    private readonly string _title;
    private readonly string _keyLabel;
    private readonly string _valueLabel;
    private readonly string _positiveButton;
    private readonly string _negativeButton;

    private readonly Action<string, string, T> _onPositiveButtonClicked;
    private readonly Action _onNegativeButtonClicked;

    private UIInputField _keyInputField;
    private UIInputField _valueInputField;
    private UIDropdown _typeDropdown;

    private readonly Enum _defaultType;
    private List<string> _descriptions = new List<string>();

    public TypedEntryDialog(
        string title = "Title",
        string keyLabel = "Key Label",
        string valueLabel = "Value Label",
        Enum defaultType = null,
        string positiveButton = "OK",
        string negativeButton = "CANCEL",
        Action<string, string, T> onPositiveButtonClicked = null,
        Action onNegativeButtonClicked = null
    )
    {
        _title = title;
        _keyLabel = keyLabel;
        _valueLabel = valueLabel;
        _defaultType = defaultType;
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

        _descriptions = Enum.GetValues(typeof(T)).Cast<T>()
            .Select(v => v.GetDescription())
            .ToList();

        _typeDropdown = UIDropdown.Create(content)
            .SetOptions(_descriptions);

        if (_defaultType != null)
        {
            int index = _descriptions.IndexOf(_defaultType.GetDescription());
            if (index != -1)
            {
                _typeDropdown.Value = index;
            }
        }

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
                                    _onPositiveButtonClicked(
                                        _keyInputField.Text,
                                        _valueInputField.Text,
                                        EnumUtils.FromDescription<T>(_descriptions[_typeDropdown.Value])
                                    );
                                })
                                .gameObject
                        )
                        .SetFlexibleWidth(1)
                };
            });
    }
}