using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Validator.Mvvm.Models
{
    /// <summary>
    /// ドキュメント状態管理列挙体
    /// </summary>
    [Flags]
    public enum DocumentStatus: int
    {
        /// <summary>
        /// 未読み込み
        /// </summary>
        None = 0,
        /// <summary>
        /// 読み込み中
        /// </summary>
        Loading = 1 << 0,
        /// <summary>
        /// 正常読み込み
        /// </summary>
        LoadedCorrectly = 1 << 1,
        /// <summary>
        /// 読み込み失敗
        /// </summary>
        LoadedFailure = 1 << 2,
        /// <summary>
        /// 検証中
        /// </summary>
        Validating = 1 << 3,
        /// <summary>
        /// 検証済み通知無し
        /// </summary>
        ValidatedNoInfo = 1 << 4,
        /// <summary>
        /// 検証済み通知あり
        /// </summary>
        ValidatedHasInfo = 1 << 5,

    }
}
