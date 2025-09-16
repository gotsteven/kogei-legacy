# プロジェクト構成

このUnityプロジェクトは、チーム開発を考慮した階層構造で整理されています。

## ディレクトリ構造

```
.
├── Assets/
│   ├── (アセットストアなどからダウンロードしたファイル群)
│   └── Project/
│       ├── Scenes/
│       │   ├── 本番用シーン
│       │   └── Test/
│       │       ├── Developer1/
│       │       │   └── Developer1のテスト用シーン
│       │       ├── Developer2/
│       │       │   └── Developer2のテスト用シーン
│       │       └── ...
│       ├── Prefabs/
│       ├── Scripts/
│       ├── Animations/
│       ├── Materials/
│       ├── PhysicsMaterials/
│       ├── Fonts/
│       ├── Textures/
│       ├── Audio/
│       ├── Resources/
│       ├── Editor/
│       └── Plugins/
└── Packages/
    └── ...
```

## フォルダの役割

### Assets/
- **ルートフォルダ**: アセットストアからダウンロードしたサードパーティ製アセットを配置（Gitで管理しない）
- **Project/**: 自作のプロジェクトファイルを配置するメインフォルダ

### Scenes/
- **本番用シーン**: リリース版で使用するシーンファイル
- **Test/**: 開発者ごとのテスト用シーンを個別フォルダで管理

### その他のフォルダ
- **Prefabs/**: 再利用可能なゲームオブジェクト
- **Scripts/**: C#スクリプトファイル
- **Animations/**: アニメーション関連ファイル
- **Materials/**: パーティクルなど
- **PhysicsMaterials/**: 物理演算用マテリアル
- **Fonts/**: UIテキスト用フォント
- **Textures/**: 画像・テクスチャファイル
- **Audio/**: BGM・SE等の音声ファイル
- **Resources/**: 実行時に動的読み込みするファイル
- **Editor/**: エディタ拡張スクリプト
- **Plugins/**: ネイティブプラグインなど

## 運用ルール

1. サードパーティ製アセットは `Assets/` 直下に配置してPushしない
2. 自作ファイルは必ず `Assets/Project/` 以下に整理
3. 個人のテスト用シーンは `Test/` 内の個人フォルダで管理
4. ファイル種別ごとに適切なフォルダに分類して配置
