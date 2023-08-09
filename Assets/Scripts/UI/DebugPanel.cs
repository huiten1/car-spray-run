using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Reflection;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] GameObject inputFieldPf;
    [SerializeField] Button saveButton;
    [SerializeField] RectTransform contentRect;
    private IEnumerator Start()
    {
        yield return null;
        var gameData = SaveManager.Load<GameData>();
        var fieldInfos = typeof(GameData).GetFields();


        float height = 0;
        foreach (var fieldInfo in fieldInfos)
        {
            Debug.Log(fieldInfo.Name);
            TMP_InputField tmpInput = InstantiateUI(gameData, ref height, fieldInfo);

            if (fieldInfo.FieldType == typeof(string))
            {
                tmpInput.contentType = TMP_InputField.ContentType.Alphanumeric;
            }
            if (fieldInfo.FieldType == typeof(int))
            {
                tmpInput.contentType = TMP_InputField.ContentType.IntegerNumber;
            }
            if (fieldInfo.FieldType == typeof(float))
            {
                tmpInput.contentType = TMP_InputField.ContentType.DecimalNumber;
            }
            if (fieldInfo.FieldType == typeof(bool))
            {

            }


            tmpInput.onValueChanged.AddListener((text) =>
            {
                try
                {
                    fieldInfo.SetValue(gameData, Convert.ChangeType(text, fieldInfo.FieldType));
                }
                catch (Exception e)
                {

                }
            });
        }

        saveButton.onClick.AddListener(() =>
        {
            SaveManager.Save(gameData);
            GameManager.Instance.Reload();
        });

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, height);
    }

    private TMP_InputField InstantiateUI(GameData gameData, ref float height, FieldInfo fieldInfo)
    {
        var inputField = Instantiate(inputFieldPf, contentRect);
        inputField.transform.GetChild(0).GetComponent<TMP_Text>().SetText(fieldInfo.Name);
        height += (inputField.transform as RectTransform).sizeDelta.y;
        var tmpInput = inputField.transform.GetChild(1).GetComponent<TMP_InputField>();

        tmpInput.transform.GetChild(0).Find("Placeholder").GetComponent<TMP_Text>().SetText(fieldInfo.GetValue(gameData).ToString());
        return tmpInput;
    }
}
