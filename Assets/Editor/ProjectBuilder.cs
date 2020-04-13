#if UNITY_EDITOR
using System; using System.Globalization;
using UnityEditor; using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.iOS;

namespace BuildExtentions
{
    public class ProjectBuilder
    {
        private const float BUILD_VERSION_PROPORTION = 0.001f; // Значение, на которое будет инкрементироваться версия при билде.
        private const int VERSION_NUMBER_AFTER_DOT = 3; // Кол-во символов после запятой, потребуется для дальнейшего форматирования строки.

        private const string ANDROID_PRODUCTION_BUILD_PATH = "Builds/Production/"; // Путь директории продукта.
        private const string ANDROID_DEBUG_BUILD_PATH = "Builds/Debugging/"; // Путь директории продукта для дебаггинга.
        private const string TEST_BUILD_PATH = "Builds/TestBuild(Trash)/"; // Путь директории продукта для дебаггинга.

        // Массив доступных сцен для билда.
        private static string[] BUILD_TARGET_SCENES = new[] { "Assets/Scenes/SampleScene.unity" };

        #region BUILD_VARIATIONS
        [MenuItem("Builder/Build All")]
        public static void Build()
        {
            Debug.Log("Builing Project!");
            // Проверка билда на наличие ошибок, данная система разработана дабы избежать инкрементации значение в случае ошибки.
            if (CheckBuildForErrors()) { Debug.LogError("Build stopped or have an errors! You have to fix it."); return; }
            SetUpVersionBuild(); // Инкрементация значения версии билда.
            //Билд
            AndroidBuild();
            AndroidDebbugBuild();
        }

        public static bool CheckBuildForErrors() {
            EditorUserBuildSettings.buildAppBundle = false; // Включить сериализацию пакета abb ( Только для Google Play ).
            PlayerSettings.Android.useAPKExpansionFiles = false; // Включить сериализацию файла с расширением Obb.
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, backend: ScriptingImplementation.Mono2x); // Устанавливаем фреймворк для нашего билда.
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7; // Смены архитектуры для целевой сборки. Оптимизация размера билда, и ускорение билдинга. 

            // API для вывода информации о билде с вводом настроек.
            BuildReport _reporter = BuildPipeline.BuildPlayer(
                BUILD_TARGET_SCENES,
                TEST_BUILD_PATH + "Trash.apk",
                BuildTarget.Android,
                BuildOptions.None
            ); 
            BuildSummary _summary = _reporter.summary;

            if (_summary.result == BuildResult.Succeeded) { Debug.Log("There's no errors while building. Build Size: " + _summary.totalSize + " bytes"); return false; }
            return true;
        }

        // Билд андройда с использование MenuItem для более простого дебагинга.
        [MenuItem("Builder/Build Android")]
        public static void AndroidBuild()
        {
            EditorUserBuildSettings.buildAppBundle = false; // Отключить сериализацию пакета abb ( Только для Google Play ).
            PlayerSettings.Android.useAPKExpansionFiles = true; // Включить сериализацию пакета Obb.
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, backend: ScriptingImplementation.IL2CPP); // Устанавливаем фреймворк для нашего билда.
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64; // Смены архитектуры для целевой сборки. Оптимизация размера билда, и ускорение билдинга. 

            // Создание экземлпяра настроек, чтобы после использовать это значение повторно (Потребуется после дебаггинга самого билда).
            BuildPlayerOptions _builderOptions = new BuildPlayerOptions();
            _builderOptions.scenes = BUILD_TARGET_SCENES;
            _builderOptions.locationPathName = ANDROID_PRODUCTION_BUILD_PATH + Application.productName + ".apk";
            _builderOptions.target = BuildTarget.Android;
            _builderOptions.options = BuildOptions.None;

            BuildPipeline.BuildPlayer(_builderOptions);
        }

        // Debugging Android Build
        [MenuItem("Builder/Build Debug Android")]
        public static void AndroidDebbugBuild()
        {
            // Первоначальный билд будет проверять на наличие ошибок, в случае если нет, то значение версии инкрементируется.

            EditorUserBuildSettings.buildAppBundle = false; // Отключить сериализацию пакета abb ( Только для Google Play ).
            PlayerSettings.Android.useAPKExpansionFiles = false; // Отключить сериализацию файла с расширением Obb.
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, backend: ScriptingImplementation.Mono2x); // Устанавливаем фреймворк для нашего билда
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7; // Смены архитектуры для целевой сборки. Оптимизация размера билда, и ускорение билдинга.

            // Создание экземлпяра настроек, чтобы после использовать это значение повторно (Потребуется после дебаггинга самого билда).
            BuildPlayerOptions _builderOptions = new BuildPlayerOptions();
            _builderOptions.scenes = BUILD_TARGET_SCENES;
            _builderOptions.locationPathName = ANDROID_DEBUG_BUILD_PATH + Application.productName + ".apk";
            _builderOptions.target = BuildTarget.Android;
            _builderOptions.options = BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging | BuildOptions.AutoRunPlayer;

            BuildPipeline.BuildPlayer(_builderOptions); // API для вывода информации о билде.
        }
        #endregion

        #region BUILD_SETTINGS
        private static void SetUpVersionBuild()
        {
            double _currentVersion = GetCurrentBundleVersion();
            double _newBuildVersion = Math.Round(_currentVersion + BUILD_VERSION_PROPORTION, VERSION_NUMBER_AFTER_DOT);
            SetNewBuildVersion(_newBuildVersion);
        }

        private static double GetCurrentBundleVersion()
        {
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            return double.Parse(PlayerSettings.bundleVersion, style: NumberStyles.Any, ci);
        }

        private static void SetNewBuildVersion(double newValue)
        {
            PlayerSettings.bundleVersion = newValue.ToString(format: "0.000", CultureInfo.InvariantCulture);
            PlayerSettings.Android.bundleVersionCode++;
        }

        // Обнуление bundle и просто версии проекта.
        [MenuItem("Builder/Reset Build Values")]
        private static void ResetVersionValues()
        {
            PlayerSettings.bundleVersion = "0.000";
            PlayerSettings.Android.bundleVersionCode = 0;
        }

        #endregion


    }
}
#endif
