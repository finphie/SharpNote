export function createEditor(dotNetHelper, elementName, language) {
    const editor = create(elementName, monaco.Uri.parse('editor'), language, "vs-dark");

    //Blazor.registerCustomEventType('didchangecontent', {
    //    browserEventName: 'didchangecontent',
    //    createEventArgs: event => {
    //        return {
    //            text: editor.getValue()
    //        };
    //    }
    //});

    editor.onDidChangeModelContent(event => {
        dotNetHelper.invokeMethodAsync('OnDidChangeContent', editor.getValue());

        // エラー表示
        // https://stackoverflow.com/questions/57994101/show-quick-fix-for-an-error-in-monaco-editor
        //const model = editor.getModel(monaco.Uri.parse('editor'));
        //const errors = GetErrors(value); // Implementation of GetErrors() not shown here

        //monaco.editor.setModelMarkers(model, "Example", errors);
    });

    monaco.languages.registerCompletionItemProvider('csharp', {
        triggerCharacters: ['.', '(', ',', ')'],
        provideCompletionItems: async function (model, position)  {
            var textUntilPosition = model.getValueInRange({ startLineNumber: 1, startColumn: 1, endLineNumber: position.lineNumber, endColumn: position.column });
            console.log(textUntilPosition);
            var cursor = textUntilPosition.length;
            var sourceInfo = { SourceCode: model.getValue(), lineNumberOffsetFromTemplate: cursor };
            console.log(sourceInfo);

            //return {
            //    'suggestions': [
            //        { 'insertText': 'Empty', 'kind': '3', 'label': 'Empty' },
            //        { 'insertText': 'Equals', 'kind': '0', 'label': 'Equals' },
            //        { 'insertText': 'NewGuid', 'kind': '0', 'label': 'NewGuid' },
            //        { 'insertText': 'Parse', 'kind': '0', 'label': 'Parse' },
            //        { 'insertText': 'ParseExact', 'kind': '0', 'label': 'ParseExact' },
            //        { 'insertText': 'ReferenceEquals', 'kind': '0', 'label': 'ReferenceEquals' },
            //        { 'insertText': 'TryParse', 'kind': '0', 'label': 'TryParse' },
            //        { 'insertText': 'TryParseExact', 'kind': '0', 'label': 'TryParseExact' }
            //    ]
            //};

            // {'suggestions': [{'insertText': 'Empty','kind':'3','label':'Empty'},{'insertText': 'Equals','kind':'0','label':'Equals'},{'insertText': 'NewGuid','kind':'0','label':'NewGuid'},{'insertText': 'Parse','kind':'0','label':'Parse'},{'insertText': 'ParseExact','kind':'0','label':'ParseExact'},{'insertText': 'ReferenceEquals','kind':'0','label':'ReferenceEquals'},{'insertText': 'TryParse','kind':'0','label':'TryParse'},{'insertText': 'TryParseExact','kind':'0','label':'TryParseExact'}]}

            //return new Promise((resolve, reject) => {
            //});

            const response = await dotNetHelper.invokeMethodAsync('GetCompletionList', model.getValue(), cursor);
            console.log(response.result);
            const completionItems = response.result;
            const suggestions = completionItems.map(item => {
                item.kind = convertKind(item.kind);
                return item;
            });

            return { 'suggestions': suggestions };
        }
    })
}

export function createViewer(elementName, language)
{
    create(elementName, monaco.Uri.parse('viewer'), language, "vs-dark", true, false);
}

export function setValue(uri, value) {
    const model = monaco.editor.getModel(monaco.Uri.parse(uri));
    model.setValue(value);
}

export function updateEditorHeight(container) {
    const height = document.documentElement.clientHeight;
    container.style.height = height + 'px';
};

const create = (elementName, modelUri, language, theme, readOnly = false, lineNumbers = true) =>
{
    // スクロールバーを非表示にする。
    // monaco.Uri.parse
    document.querySelector('body').style.overflow = 'hidden';

    const container = document.getElementById(elementName);
    const model = monaco.editor.createModel('', language, modelUri);
    const editor = monaco.editor.create(container, {
        model: model,
        automaticLayout: true,
        minimap: {
            enabled: false
        },
        scrollBeyondLastLine: false,
        language: language,
        theme: theme,
        readOnly: readOnly,
        lineNumbers: lineNumbers,
    });

    updateEditorHeight(container);
    window.addEventListener('resize', () => updateEditorHeight(container));

    return editor;
}

// TODO: parse
const convertKind = kind => monaco.languages.CompletionItemKind[kind];
