using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzySharp;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using NPinyin;

namespace CodeHelper
{
    public class SearchHelper
    {
        /// <summary>
        /// 根据输入内容模糊搜索
        /// </summary>
        public static List<KeyValuePair<string, object>> GetFuzzySearchResult(string input, List<KeyValuePair<string, object>> data, double fitratio = 0.6)
        {
            var pinyinstrs = data.Select((x) => Pinyin.GetPinyin(x.Key)).ToList();
            var inputpinyin = Pinyin.GetPinyin(input);
            return Process.ExtractAll(inputpinyin, pinyinstrs, scorer: new PartialRatioScorer()).Where((x) => x.Score > 100 * fitratio).OrderByDescending((x) => x.Score).Select((x) => data[pinyinstrs.IndexOf(x.Value)]).Select((x) => new KeyValuePair<string, object>(x.Key, x.Value)).ToList();
        }
    }
}
