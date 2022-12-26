using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static Dictionary<(int, int), int> Read(string file)
    {
        var dic = new Dictionary<(int, int), int>();   // (���id, �����ⱸid), ���뿩��     // ���뿩�� = ��ᰡ �����ⱸ���� �丮�� �������� ����
        TextAsset data = Resources.Load(file) as TextAsset;

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);  // Regex = ����ǥ����, ����ó�� ���������� ó���ϴ� Ŭ����       // ���� : https://learn.microsoft.com/ko-kr/dotnet/api/system.text.regularexpressions.regex?view=net-7.0

        if (lines.Length <= 1) return null;
        var header = Regex.Split(lines[0], SPLIT_RE);   // ù ��° ���� ����� �Ǵ�, ���� ������ŭ ������� (���ϻ� ������Ʈ �������δ� ����̸�, ���id, �����ⱸ�̸�, �����ⱸid, �������ɿ���)

        for (var i = 1; i < lines.Length; i++)          // �� ��° ����� ����
        {
            var values = Regex.Split(lines[i], SPLIT_RE);   // ���������� �� ĭ
            if (values.Length == 0 || values[0] == "") continue;    // �ش� ĭ�� ��������� �Ѿ

            // �ش� ������Ʈ ����

            // ���������� �� ��°(B) �� (=���id) �Ľ��ϱ�
            string ingID = values[1];
            ingID = ingID.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");     // ���� ����
            int ingredientID = -1, n;
            // �� ĭ ���� �ִ� ���ڿ��� ������ �Ľ��ϴ� ����
            if (int.TryParse(ingID, out n)) // ������ ���� �Ľ̵Ǹ� n�� ������ ����        // ���� �Ľ�? : int�� �Ľ��� �� �ִ� ���̾�� �Ľ��� ��
            {
                ingredientID = n;
            }

            // ���������� �� ��°(D) �� (=�����ⱸid)  �Ľ��ϱ�
            string appID = values[3];
            appID = appID.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
            int applianceID = -1;
            // �� ĭ ���� �ִ� ���ڿ��� ������ �Ľ��ϴ� ����
            if (int.TryParse(appID, out n)) // ������ ���� �Ľ̵Ǹ� n�� ������ ����        // ���� �Ľ�? : int�� �Ľ��� �� �ִ� ���̾�� �Ľ��� ��
            {
                applianceID = n;
            }

            var tupleID = (ingredientID, applianceID);  // ��ųʸ� key���� �Ǵ� ���id, �����ⱸid Ʃ��

            // �������� �ټ� ��°(E) �� (=�������� ����) �Ľ��ϱ�
            string able = values[4];
            able = able.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
            int cookable = -1;
            // �� ĭ ���� �ִ� ���ڿ��� ������ �Ľ��ϴ� ����
            if (int.TryParse(able, out n)) // ������ ���� �Ľ̵Ǹ� n�� ������ ����        // ���� �Ľ�? : int�� �Ľ��� �� �ִ� ���̾�� �Ľ��� ��
            {
                cookable = n;
            }

            dic[tupleID] = cookable;  // key : (���id, �����ⱸid)Ʃ��, value : ���뿩��
        }
        return dic;
    }
}
