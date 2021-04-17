using UnityEngine;

public static class LoadText
{
    public static string[] textMessage; //テキストの加工前の一行を入れる変数

    private static int rowLength; //テキスト内の行数を取得する変数
    private static int columnLength; //テキスト内の列数を取得する変数
    private static string TextLines;

    private static char splitChara = ',';

    //TextAseetの読み込み
    public static string LoadTextData(string textName)
    {
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("Stages/00MapDates/" + textName, typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        return textasset.text;
    }

    //二次元配列への代入
    public static string[,] SetTexts(string textData, string[,] stringarry)
    {
        TextLines = textData;
        textMessage = TextLines.Split('\n');

        //行数と列数を取得
        rowLength = textMessage.Length;
        columnLength = textMessage[0].Split(splitChara).Length;
        //Debug.Log(columnLength);

        //2次配列を定義
        stringarry = new string[rowLength, columnLength];

        for (int i = 0; i < rowLength; i++)
        {
            string[] tempWords = textMessage[i].Trim().Split(splitChara); //textMessageをsplitCharaごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < columnLength; n++)
            {
                if (tempWords[n] == string.Empty)
                {
                    break;
                }
                stringarry[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }
        return stringarry;
    }

    public static string[,] SetTexts(string textData)
    {
        string[,] stringarry;
        TextLines = textData;
        textMessage = TextLines.Split('\n');

        //行数と列数を取得
        rowLength = textMessage.Length;
        columnLength = textMessage[0].Split(splitChara).Length;

        //2次配列を定義
        stringarry = new string[rowLength, columnLength];

        for (int i = 0; i < rowLength; i++)
        {
            string[] tempWords = textMessage[i].Split(splitChara); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < columnLength; n++)
            {
                if (tempWords[n] == string.Empty)
                {
                    break;
                }
                stringarry[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
                //Debug.Log(tempWords[n]);
            }
        }
        return stringarry;
    }
}
