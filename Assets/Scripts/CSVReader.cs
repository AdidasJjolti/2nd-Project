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
        var dic = new Dictionary<(int, int), int>();   // (재료id, 조리기구id), 가용여부     // 가용여부 = 재료가 조리기구에서 요리가 가능한지 여부
        TextAsset data = Resources.Load(file) as TextAsset;

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);  // Regex = 정규표현식, 엑셀처럼 문자조건을 처리하는 클래스       // 참고 : https://learn.microsoft.com/ko-kr/dotnet/api/system.text.regularexpressions.regex?view=net-7.0

        if (lines.Length <= 1) return null;
        var header = Regex.Split(lines[0], SPLIT_RE);   // 첫 번째 행은 헤더로 판단, 열의 개수만큼 집어넣음 (규하샘 프로젝트 기준으로는 재료이름, 재료id, 조리기구이름, 조리기구id, 조리가능여부)

        for (var i = 1; i < lines.Length; i++)          // 두 번째 행부터 시작
        {
            var values = Regex.Split(lines[i], SPLIT_RE);   // 엑셀에서의 한 칸
            if (values.Length == 0 || values[0] == "") continue;    // 해당 칸이 비어있으면 넘어감

            // 해당 프로젝트 버전

            // 엑설에서의 두 번째(B) 열 (=재료id) 파싱하기
            string ingID = values[1];
            ingID = ingID.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");     // 공백 제거
            int ingredientID = -1, n;
            // 한 칸 내에 있는 문자열을 정수로 파싱하는 과정
            if (int.TryParse(ingID, out n)) // 정수로 정상 파싱되면 n에 정수값 저장        // 정상 파싱? : int로 파싱할 수 있는 값이어야 파싱이 됨
            {
                ingredientID = n;
            }

            // 엑설에서의 네 번째(D) 열 (=조리기구id)  파싱하기
            string appID = values[3];
            appID = appID.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
            int applianceID = -1;
            // 한 칸 내에 있는 문자열을 정수로 파싱하는 과정
            if (int.TryParse(appID, out n)) // 정수로 정상 파싱되면 n에 정수값 저장        // 정상 파싱? : int로 파싱할 수 있는 값이어야 파싱이 됨
            {
                applianceID = n;
            }

            var tupleID = (ingredientID, applianceID);  // 딕셔너리 key값이 되는 재료id, 조리기구id 튜플

            // 엑셀에서 다섯 번째(E) 열 (=조리가능 여부) 파싱하기
            string able = values[4];
            able = able.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
            int cookable = -1;
            // 한 칸 내에 있는 문자열을 정수로 파싱하는 과정
            if (int.TryParse(able, out n)) // 정수로 정상 파싱되면 n에 정수값 저장        // 정상 파싱? : int로 파싱할 수 있는 값이어야 파싱이 됨
            {
                cookable = n;
            }

            dic[tupleID] = cookable;  // key : (재료id, 조리기구id)튜플, value : 가용여부
        }
        return dic;
    }

    public static Dictionary<(int, int, int), int> ReadCompleteFood(string file)
    {
        var dic = new Dictionary<(int, int, int), int>();   // (재료1 id, 재료2 id, 재료3 id), 완성 음식     // 재료 id가 -1이면 빈 영역
        TextAsset data = Resources.Load(file) as TextAsset;

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);  // Regex = 정규표현식, 엑셀처럼 문자조건을 처리하는 클래스       // 참고 : https://learn.microsoft.com/ko-kr/dotnet/api/system.text.regularexpressions.regex?view=net-7.0

        if (lines.Length <= 1) return null;
        var header = Regex.Split(lines[0], SPLIT_RE);   // 첫 번째 행은 헤더로 판단, 열의 개수만큼 집어넣음 (규하샘 프로젝트 기준으로는 재료이름, 재료id, 조리기구이름, 조리기구id, 조리가능여부)

        for (var i = 1; i < lines.Length; i++)          // 두 번째 행부터 시작
        {
            var values = Regex.Split(lines[i], SPLIT_RE);   // 엑셀에서의 한 칸
            if (values.Length == 0 || values[0] == "") continue;    // 해당 칸이 비어있으면 넘어감

            // 해당 프로젝트 버전

            // 엑설에서의 두 번째(B) 열 (=재료1 id) 파싱하기
            string ing1ID = values[1];
            ing1ID = ing1ID.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");     // 공백 제거
            int ingredient1ID = -1, n;
            // 한 칸 내에 있는 문자열을 정수로 파싱하는 과정
            if (int.TryParse(ing1ID, out n)) // 정수로 정상 파싱되면 n에 정수값 저장        // 정상 파싱? : int로 파싱할 수 있는 값이어야 파싱이 됨
            {
                ingredient1ID = n;
            }

            // 엑설에서의 네 번째(D) 열 (=재료2 id)  파싱하기
            string ing2ID = values[3];
            ing2ID = ing2ID.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
            int ingredient2ID = -1;
            // 한 칸 내에 있는 문자열을 정수로 파싱하는 과정
            if (int.TryParse(ing2ID, out n)) // 정수로 정상 파싱되면 n에 정수값 저장        // 정상 파싱? : int로 파싱할 수 있는 값이어야 파싱이 됨
            {
                ingredient2ID = n;
            }

            // 엑설에서의 여섯 번째(D) 열 (=재료3 id)  파싱하기
            string ing3ID = values[5];
            ing3ID = ing3ID.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
            int ingredient3ID = -1;
            // 한 칸 내에 있는 문자열을 정수로 파싱하는 과정
            if (int.TryParse(ing3ID, out n)) // 정수로 정상 파싱되면 n에 정수값 저장        // 정상 파싱? : int로 파싱할 수 있는 값이어야 파싱이 됨
            {
                ingredient3ID = n;
            }

            List<int> tupleList = new List<int>();
            tupleList.Add(ingredient1ID);
            tupleList.Add(ingredient2ID);
            tupleList.Add(ingredient3ID);
            tupleList.Sort();                                          // 오름차순으로 id 정렬

            var tupleID = (tupleList[0], tupleList[1], tupleList[2]);  // 딕셔너리 key값이 되는 재료1 id, 재료2 id, 재료3 id 튜플
            //Debug.Log($"{tupleList[0]}, {tupleList[1]}, {tupleList[2]}");

            // 엑셀에서 여덟 번째(E) 열 (=음식) 파싱하기
            string food = values[7];
            food = food.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
            int completefood = -1;
            // 한 칸 내에 있는 문자열을 정수로 파싱하는 과정
            if (int.TryParse(food, out n)) // 정수로 정상 파싱되면 n에 정수값 저장        // 정상 파싱? : int로 파싱할 수 있는 값이어야 파싱이 됨
            {
                completefood = n;
            }

            dic[tupleID] = completefood;  // key : (재료1 id, 재료2 id, 재료3 id)튜플, value : 완성된 음식 id
        }
        return dic;
    }
}
