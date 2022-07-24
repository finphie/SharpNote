using System.Text.Json.Serialization;

namespace SharpNote.Core;

/// <summary>
/// 入力補完の種類
/// <see href="https://microsoft.github.io/monaco-editor/api/enums/monaco.languages.CompletionItemKind.html"/>
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CompletionItemKind
{
    /// <summary>
    /// クラス
    /// </summary>
    Class,

    /// <summary>
    /// カラー
    /// </summary>
    Color,

    /// <summary>
    /// 定数
    /// </summary>
    Constant,

    /// <summary>
    /// コンストラクター
    /// </summary>
    Constructor,

    /// <summary>
    /// カスタムカラー
    /// </summary>
    Customcolor,

    /// <summary>
    /// 列挙型
    /// </summary>
    Enum,

    /// <summary>
    /// 列挙型のメンバー
    /// </summary>
    EnumMember,

    /// <summary>
    /// イベント
    /// </summary>
    Event,

    /// <summary>
    /// フィールド
    /// </summary>
    Field,

    /// <summary>
    /// ファイル
    /// </summary>
    File,

    /// <summary>
    /// フォルダ
    /// </summary>
    Folder,

    /// <summary>
    /// 関数
    /// </summary>
    Function,

    /// <summary>
    /// インターフェイス
    /// </summary>
    Interface,

    /// <summary>
    /// 問題
    /// </summary>
    Issue,

    /// <summary>
    /// キーワード
    /// </summary>
    Keyword,

    /// <summary>
    /// メソッド
    /// </summary>
    Method,

    /// <summary>
    /// モジュール
    /// </summary>
    Module,

    /// <summary>
    /// 演算子
    /// </summary>
    Operator,

    /// <summary>
    /// プロパティ
    /// </summary>
    Property,

    /// <summary>
    /// リファレンス
    /// </summary>
    Reference,

    /// <summary>
    /// スニペット
    /// </summary>
    Snippet,

    /// <summary>
    /// 構造体
    /// </summary>
    Struct,

    /// <summary>
    /// テキスト
    /// </summary>
    Text,

    /// <summary>
    /// 型引数
    /// </summary>
    TypeParameter,

    /// <summary>
    /// ユニット
    /// </summary>
    Unit,

    /// <summary>
    /// ユーザー
    /// </summary>
    User,

    /// <summary>
    /// 値
    /// </summary>
    Value,

    /// <summary>
    /// 変数
    /// </summary>
    Variable
}
