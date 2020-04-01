using UnityEngine;
using System.Collections.Generic;

// This could be done with external files I GUESS
public class TextManager {

    private static Dictionary<string, string> texts = new Dictionary<string, string>();

    public static string GetText(string index) {
        var text = index;

        if (!texts.ContainsKey(index)) {
            var allTexts = GetAllTexts();
            if (allTexts.ContainsKey(index)) {
                var possibles = allTexts[index];
                if (possibles.ContainsKey(GameState.lang)) {
                    text = possibles[GameState.lang];
                }
            }

            texts.Add(index, text);
        }

        return texts[index];
    }

    public static void CleanCache() {
        texts = new Dictionary<string, string>();
    }

    private static Dictionary<string, Dictionary<SystemLanguage, string>> GetAllTexts() {
        return new Dictionary<string, Dictionary<SystemLanguage, string>>() {
            { "intro", IntroTexts() },
            { "options", OptionsTexts() },
            { "language", LanguageTexts() },
            { "difficulty", DifficultyTexts() },
            { "veryeasy", VeryEasyTexts() },
            { "easy", EasyTexts() },
            { "medium", MediumTexts() },
            { "hard", HardTexts() },
            { "extreme", ExtremeTexts() },
            { "back", BackTexts() },
            { "tutorial", TutorialTexts() },
            { "start game", StartGameTexts() },
            { "continue game", ContinueGameTexts() },
            { "exit game", ExitGameTexts() },
            { "score", ScoreTexts() },
            { "wafers", WaferTexts() },
            { "time", TimeTexts() },
            { "resume", ResumeTexts() },
            { "main menu", MainMenuTexts() },
            { "menu exit game", MenuExitGameTexts() },
            { "subtitle", SubtitileText() },
            { "sfx volume", SfxVolumeTexts() },
            { "music volume", MusicVolumeTexts() },
            { "configure controls", ConfigureControlsTexts() },
            { "press key", PressKeyTexts() },
            { "press", PressStartTexts() },
            { "keyboard", KeyboardTexts() },
            { "gamepad", GamepadTexts() },
            { "jump", JumpTexts() },
            { "shoot", ShootTexts() },
            { "look up", LookUpTexts() },
            { "crouch", CrouchTexts() },
            { "move left", MoveLeftTexts() },
            { "move right", MoveRightTexts() },
            { "loading", LoadingTexts() },
            { "game finished", GameEndTexts() },
            { "act finished", ActEndTexts() },
            { "time bonus", TimeBonusTexts() },
            { "clear bonus", ClearBonusTexts() },
            { "wafer bonus", WaferBonusTexts() },
            { "monastery_boss_name", MonasteryBossName() },
            { "monastery_boss_text", MonasteryBossTexts() },
            { "nightbar_boss_name", NightbarBossName() },
            { "nightbar_boss_text", NightbarBossTexts() },
            { "hospital_boss_name", HospitalBossName() },
            { "hospital_boss_text", HospitalBossTexts() },
            { "desert_boss_name", DesertBossName() },
            { "desert_boss_text", DesertBossTexts() },
            { "lab_boss_name", LabBossTexts() },
            { "lab_boss_text", LabBossTexts() },
            { "hq_boss_name", HqBossName() },
            { "commie_boss_text", CommieBossTexts() },
            { "peter_boss_name", PeterBossName() },
            { "peter_boss_text", PeterTexts() },
            { "lenin_boss_name", LeninBossName() },
            { "lenin_boss_text", LeninfaceTexts() },
            { "marxex_boss_name", MarxexBossName() },
            { "marxex_boss_text", MarxexTexts() },
            { "jesus_boss_name", JesusBossName() },
            { "jesus_boss_text", JesusTexts() },
            { "hold", HoldTexts() },
        };
    }

    // Intro.

    private static Dictionary<SystemLanguage, string> IntroTexts() {
        return new Dictionary<SystemLanguage, string> {
            { SystemLanguage.Spanish,
                "Año 2020\n\n"
                + "Tras su resurgimiento desde el infierno, las hordas comunistas han tomado el control del congreso.\n"
                + "Los rojos se han apoderado del país, provocando que demonios tomen posesión de inocentes creyentes.\n"
                + "El anticristo, marcado con la hoz y el martillo, ha traído el infierno a la tierra.\n"
                + "Las iglesias arden, las personas de bien mueren...\n"
                + "Pero hay una monja que no piensa permitir que el mal campe a sus anchas sobre la tierra.\n"
                + "Una monja que, con rosario colgado y cruz y agua bendita en mano, piensa purificar el mundo.\n"
                + "Una monja que no se detendrá ante nada."
            },
            { SystemLanguage.English,
                "Year of our Lord 2020 A.D.\n\n"
                + "After their resurgence from hell, the communist hordes have taken control of the government.\n"
                + "The red menace has taken power over the country, provoking demons to possess innocent Christians.\n"
                + "The Antichrist, marked by the sickle and hammer, has brought Hell to Earth.\n"
                + "The churchs are burning... the worthy are dying...\n"
                + "But there's one nun, who shan't allow this evil to roam freely over the surface of the planet.\n"
                + "One nun, who armed with rosary beads, cross, and holy water in hand, will purify the world.\n"
                + "One nun, who will stop at nothing."
            },
            { SystemLanguage.Portuguese,
                "Ano 2020\n\n"
                + "Após seu ressurgimento do inferno, as hordas comunistas tomaram o controle do Congresso.\n"
                + "Os comunistas têm tomado todo o país, causando demônios tomar posse dos crentes inocentes.\n"
                + "Anticristo, marcado com um rabo de cavalo, trouxe o inferno à terra.\n"
                + "Igrejas arden, boas pessoas morrem...\n"
                + "Mas há uma freira que não pensa permitindo campeão mal à vontade na terra.\n"
                + "Uma freira que, com gancho e cruz e água benta em rosário lado, pensa purificar o mundo.\n"
                + "Uma freira que vai parar em nada."
            },
            { SystemLanguage.French,
                "Année 2020\n\n"
                + "Après leur renaissance de l'enfer, des hordes de communistes ont pris le contrôle du Congrès.\n"
                + "Les gauchistes se sont emparé du pays laissant ainsi les démons prendre possession de croyants innocents.\n"
                + "L'Antéchrist, reconnaissable à sa queue de cheval, a amené l'enfer sur terre.\n"
                + "Les églises brûlent, les honnêtes gens meurent...\n"
                + "Cependant, une religieuse se refuse à laisser le mal opérer en tout impunité sur la planète.\n"
                + "Une religieuse qui, avec un rosaire, une croix et de l'eau bénite en main, envisage de purifier le monde.\n"
                + "Une religieuse qui ne s'arrêtera devant rien."
            },
            { SystemLanguage.German,
                "Im Jahre des Herrn 2020\n\n"
                + "Nach ihrer Wiederkehr aus der Hölle haben die kommunistischen Horden die Kontrolle über die Regierung übernommen.\n"
                + "Die rote Bedrohung hat die Macht über das Land genommen und provoziert Dämonen unschuldige Christen zu besessen.\n"
                + "Der von der Sichel und dem Hammer geprägte Antichrist hat die Hölle zur Erde gebracht.\n"
                + "Die Kirchen brennen... die Würdigen sterben...\n"
                + "Aber es gibt eine Nonne die nicht erlauben wird, dass dieses Böse frei über die Oberfläche des Planeten wandern darf.\n"
                + "Eine Nonne, mit Rosenkranz, Kreuz und Weihwasser in der Hand bewaffnet, wird diese Welt läutern.\n"
                + "Eine Nonne, die vor nichts halt machen wird."
            },
            { SystemLanguage.Catalan,
                "Any 2020\n\n"
                + "Rere el seu ressorgiment des de l'infern, les hordes comunistes han pres el control del congrés.\n"
                + "Els rojos s'han apoderat del país, provocant que demonis prenguin possessió d'innocents creients.\n"
                + "L'anticrist, marcat amb la falç i el martell, ha portat l'infern a la terra.\n"
                + "Les esglésies cremen, les persones de bé moren...\n"
                + "Però hi ha una monja que no pensa permetre que el mal campi al seu aire sobre la terra.\n"
                + "Una monja que, amb rosari penjat i creu i agua beneïda en mà, pensa purificar el món.\n"
                + "Una monja que no s'aturarà davant de res."
            },
            { SystemLanguage.Basque,
                "2020 urtea\n\n"
                + "Infernutikako haien berpiztearen ostean, horda komunistek kongresuaren kontrola hartu dute.\n"
                + "Gorriak herrialdeaz jabetu dira, deabruek sinesle errugabeak hartu dituzte.\n"
                + "Antikristoak, ile-motots batez markatua, Infernua Lurrera ekarri du.\n"
                + "Elizak erretzen eta pertsona zintzoak hiltzen dira...\n"
                + "Baina bada gaizkiak libreki lurra estaltzea onartuko ez duen monja bat.\n"
                + "Errosarioa zintzilik eta gurutzea eta ur bedeinkatua eskutan, mundua araztuko duen monja bat.\n"
                + "Ezeren aurrean geldituko ez den monja bat."
            },
            { SystemLanguage.Japanese,
                "時に,西暦2020年"
                + "地獄から立ち上がった後, 共産主義の大群が政府を支配してきた。\n"
                + "赤い脅威は国を支配しています、無邪気なクリスチャンを所有するために悪魔を誘発。\n"
                + "反キリスト、鎌とハンマーでマーク、地獄を地球にもたらした。\n"
                + "教会は燃えています... ふさわしいのは死につつある...\n"
                + "しかし、一人の修道女がいる、この悪が惑星の表面を自由に歩き回るのを許さない人。\n"
                + "一修道女、ロザリオビーズ、十字架、そして聖水を手にして武装した者は、世界を浄化する。\n"
                + "一修道女、誰もが止まらない。"
            }
        };
    }

    // Main menu.

    private static Dictionary<SystemLanguage, string> SubtitileText() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "La mujer de Dios es ahora el puño de Dios" },
            { SystemLanguage.English, "The sister of God is now the fist of God" },
            { SystemLanguage.Portuguese, "A irmã de Deus é agora o punho de Deus" },
            { SystemLanguage.French, "La soeur de Dieu est le poing de Dieu" },
            { SystemLanguage.German, "Die Schwester Gottes ist nun die Faust Gottes" },
            { SystemLanguage.Catalan, "La dona de Déu es ara el puny de Déu" },
            { SystemLanguage.Basque, "Jainkoaren emaztea Jainkoaren ukabila da orain" },
            { SystemLanguage.Japanese, "神の姉妹は今神の拳です" }
        };
    }

    private static Dictionary<SystemLanguage, string> TutorialTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Tutorial" },
            { SystemLanguage.English, "Tutorial" },
            { SystemLanguage.Portuguese, "Tutorial" },
            { SystemLanguage.French, "Tutorial" },
            { SystemLanguage.German, "Tutorial" },
            { SystemLanguage.Catalan, "Tutorial" },
            { SystemLanguage.Basque, "Tutoretza" },
            { SystemLanguage.Japanese, "チュートリアル" }
        };
    }

    private static Dictionary<SystemLanguage, string> StartGameTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Empezar" },
            { SystemLanguage.English, "Start Game" },
            { SystemLanguage.Portuguese, "Començar" },
            { SystemLanguage.French, "Démarrer jeu" },
            { SystemLanguage.German, "Spiel starten" },
            { SystemLanguage.Catalan, "Jugar" },
            { SystemLanguage.Basque, "Jolastu" },
            { SystemLanguage.Japanese, "プレー" }
        };
    }

    private static Dictionary<SystemLanguage, string> ContinueGameTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Continuar" },
            { SystemLanguage.English, "Continue" },
            { SystemLanguage.Portuguese, "Continuar" },
            { SystemLanguage.French, "Continuer" },
            { SystemLanguage.German, "Fortsetzen" },
            { SystemLanguage.Catalan, "Continuar" },
            { SystemLanguage.Basque, "Jarraitu" },
            { SystemLanguage.Japanese, "コンティニュー" }
        };
    }

    private static Dictionary<SystemLanguage, string> ExitGameTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Salir del Juego" },
            { SystemLanguage.English, "Exit Game" },
            { SystemLanguage.Portuguese, "Sair do Jogo" },
            { SystemLanguage.French, "Quitter le jeu" },
            { SystemLanguage.German, "Spiel verlassen" },
            { SystemLanguage.Catalan, "Sortir del Joc" },
            { SystemLanguage.Basque, "Jolasetik irten" },
            { SystemLanguage.Japanese, "ストップゲーム" }
        };
    }

    // Options.

    private static Dictionary<SystemLanguage, string> OptionsTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Opciones" },
            { SystemLanguage.English, "Options" },
            { SystemLanguage.Portuguese, "Opções" },
            { SystemLanguage.French, "Options" },
            { SystemLanguage.German, "Einstellungen" },
            { SystemLanguage.Catalan, "Opcions" },
            { SystemLanguage.Basque, "Aukerak" },
            { SystemLanguage.Japanese, "オプション" }
        };
    }

    private static Dictionary<SystemLanguage, string> LanguageTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Idioma" },
            { SystemLanguage.English, "Language" },
            { SystemLanguage.Portuguese, "Idioma" },
            { SystemLanguage.French, "Langue" },
            { SystemLanguage.German, "Sprache" },
            { SystemLanguage.Catalan, "idioma" },
            { SystemLanguage.Basque, "Hizkuntza" },
            { SystemLanguage.Japanese, "言語" }
        };
    }

    private static Dictionary<SystemLanguage, string> DifficultyTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Dificultad" },
            { SystemLanguage.English, "Difficulty" },
            { SystemLanguage.Portuguese, "Dificuldade" },
            { SystemLanguage.French, "Difficulté" },
            { SystemLanguage.German, "Schwierigkeit" },
            { SystemLanguage.Catalan, "Dificultat" },
            { SystemLanguage.Basque, "Zailtasun" },
            { SystemLanguage.Japanese, "難易度" }
        };
    }

    private static Dictionary<SystemLanguage, string> VeryEasyTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Muy Fácil" },
            { SystemLanguage.English, "Very Easy" },
            { SystemLanguage.Portuguese, "Muito Fácil" },
            { SystemLanguage.French, "Très facile" },
            { SystemLanguage.German, "Sehr Leicht" },
            { SystemLanguage.Catalan, "Molt Fàcil" },
            { SystemLanguage.Basque, "Oso Erraza" },
            { SystemLanguage.Japanese, "易々" }
        };
    }

    private static Dictionary<SystemLanguage, string> EasyTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Fácil" },
            { SystemLanguage.English, "Easy" },
            { SystemLanguage.Portuguese, "Fácil" },
            { SystemLanguage.French, "Facile" },
            { SystemLanguage.German, "Leicht" },
            { SystemLanguage.Catalan, "Fàcil" },
            { SystemLanguage.Basque, "Erraza" },
            { SystemLanguage.Japanese, "易い" }
        };
    }

    private static Dictionary<SystemLanguage, string> MediumTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Medio" },
            { SystemLanguage.English, "Medium" },
            { SystemLanguage.Portuguese, "Meio" },
            { SystemLanguage.French, "Moyen" },
            { SystemLanguage.German, "Mittel" },
            { SystemLanguage.Catalan, "Mitjana" },
            { SystemLanguage.Basque, "Bitartekoak" },
            { SystemLanguage.Japanese, "並み" }
        };
    }

    private static Dictionary<SystemLanguage, string> HardTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Difícil" },
            { SystemLanguage.English, "Hard" },
            { SystemLanguage.Portuguese, "Difícil" },
            { SystemLanguage.French, "Difficile" },
            { SystemLanguage.German, "Schwer" },
            { SystemLanguage.Catalan, "Difícil" },
            { SystemLanguage.Basque, "Zaila" },
            { SystemLanguage.Japanese, "難い" }
        };
    }

    private static Dictionary<SystemLanguage, string> ExtremeTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Extremo" },
            { SystemLanguage.English, "Extreme" },
            { SystemLanguage.Portuguese, "Muito difícil" },
            { SystemLanguage.French, "Extrême" },
            { SystemLanguage.German, "Extrem" },
            { SystemLanguage.Catalan, "Extrema" },
            { SystemLanguage.Basque, "Oso zaila" },
            { SystemLanguage.Japanese, "激しい" }
        };
    }

    private static Dictionary<SystemLanguage, string> BackTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Volver" },
            { SystemLanguage.English, "Back" },
            { SystemLanguage.Portuguese, "Retorno" },
            { SystemLanguage.French, "Arrière" },
            { SystemLanguage.German, "Zurück" },
            { SystemLanguage.Catalan, "Tornar"},
            { SystemLanguage.Basque, "Bueltan" },
            { SystemLanguage.Japanese, "戻る" }
        };
    }

    private static Dictionary<SystemLanguage, string> SfxVolumeTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Volumen de SFX" },
            { SystemLanguage.English, "SFX Volume" },
            { SystemLanguage.Portuguese, "Volume de SFX" },
            { SystemLanguage.French, "Volume SFX" },
            { SystemLanguage.German, "SFX Lautstärke" },
            { SystemLanguage.Catalan, "Volum de SFX" },
            { SystemLanguage.Basque, "SFX Bolumena" },
            { SystemLanguage.Japanese, "SFXのボリューム" }
        };
    }

    private static Dictionary<SystemLanguage, string> MusicVolumeTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Volumen de Música" },
            { SystemLanguage.English, "Music Volume" },
            { SystemLanguage.Portuguese, "Volume de Música" },
            { SystemLanguage.French, "Volume de la musique" },
            { SystemLanguage.German, "Musiklautstärke" },
            { SystemLanguage.Catalan, "Volum de Música" },
            { SystemLanguage.Basque, "Musika Bolumena" },
            { SystemLanguage.Japanese, "音楽のボリューム" }
        };
    }

    // In-game texts.

    private static Dictionary<SystemLanguage, string> ScoreTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Puntuación" },
            { SystemLanguage.English, "Score" },
            { SystemLanguage.Portuguese, "Pontos" },
            { SystemLanguage.French, "Score" },
            { SystemLanguage.German, "Punkte" },
            { SystemLanguage.Catalan, "Puntuació" },
            { SystemLanguage.Basque, "Puntuazio" },
            { SystemLanguage.Japanese, "スコア" }
        };
    }

    private static Dictionary<SystemLanguage, string> TimeTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Tiempo" },
            { SystemLanguage.English, "Time" },
            { SystemLanguage.Portuguese, "Tempo" },
            { SystemLanguage.French, "Temps" },
            { SystemLanguage.German, "Zeit" },
            { SystemLanguage.Catalan, "Temps" },
            { SystemLanguage.Basque, "Denbora" },
            { SystemLanguage.Japanese, "時間" }
        };
    }

    private static Dictionary<SystemLanguage, string> WaferTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Obleas" },
            { SystemLanguage.English, "Wafers" },
            { SystemLanguage.Portuguese, "Hóstias" },
            { SystemLanguage.French, "Gaufrettes" },
            { SystemLanguage.German, "Hostien" },
            { SystemLanguage.Catalan, "Hòsties" },
            { SystemLanguage.Basque, "Eragiten" },
            { SystemLanguage.Japanese, "ウェファ" }
        };
    }

    private static Dictionary<SystemLanguage, string> ResumeTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Seguir Jugando" },
            { SystemLanguage.English, "Resume" },
            { SystemLanguage.Portuguese, "Retorna" },
            { SystemLanguage.French, "Reprendre" },
            { SystemLanguage.German, "Fortsetzen" },
            { SystemLanguage.Catalan, "Tornar" },
            { SystemLanguage.Basque, "Bueltan" },
            { SystemLanguage.Japanese, "再開する" }
        };
    }

    private static Dictionary<SystemLanguage, string> MainMenuTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Menú Principal" },
            { SystemLanguage.English, "Main Menu" },
            { SystemLanguage.Portuguese, "Menu Principal" },
            { SystemLanguage.French, "Menu Principal" },
            { SystemLanguage.German, "Hauptmenü" },
            { SystemLanguage.Catalan, "Menú Principal" },
            { SystemLanguage.Basque, "Menu Nagusia" },
            { SystemLanguage.Japanese, "メインメニュー" }
        };
    }

    private static Dictionary<SystemLanguage, string> MenuExitGameTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Salir del Juego" },
            { SystemLanguage.English, "Exit Game" },
            { SystemLanguage.Portuguese, "Sair do Jogo" },
            { SystemLanguage.French, "Quitter le jeu" },
            { SystemLanguage.German, "Spiel verlassen" },
            { SystemLanguage.Catalan, "Sortir del Joc" },
            { SystemLanguage.Basque, "Irteera" },
            { SystemLanguage.Japanese, "出口ゲーム" }
        };
    }

    // Configure controls menu

    private static Dictionary<SystemLanguage, string> ConfigureControlsTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Configurar Controles" },
            { SystemLanguage.English, "Configure Controls" },
            { SystemLanguage.Portuguese, "Configurar Controles" },
            { SystemLanguage.French, "Configurer les contrôles" },
            { SystemLanguage.German, "Steuerungskonfiguration" },
            { SystemLanguage.Catalan, "Configurar Controls" },
            { SystemLanguage.Basque, "Konfiguratu Kontrolak" },
            { SystemLanguage.Japanese, "コントロールを設定する" }
        };
    }

    private static Dictionary<SystemLanguage, string> PressKeyTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Pulsa tecla" },
            { SystemLanguage.English, "Press key" },
            { SystemLanguage.Portuguese, "Pressione tecla" },
            { SystemLanguage.French, "Presse touche" },
            { SystemLanguage.German, "Drücken Taste" },
            { SystemLanguage.Catalan, "Prem tecla" },
            { SystemLanguage.Basque, "Sakatu tekla" },
            { SystemLanguage.Japanese, "キーをプレス" }
        };
    }

    private static Dictionary<SystemLanguage, string> PressStartTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Pulsa Start ({0})" },
            { SystemLanguage.English, "Press Start ({0})" },
            { SystemLanguage.Portuguese, "Pressione Start ({0})" },
            { SystemLanguage.French, "Presse Start ({0})" },
            { SystemLanguage.German, "Drücken Start ({0})" },
            { SystemLanguage.Catalan, "Prem Start ({0})" },
            { SystemLanguage.Basque, "Sakatu Start ({0})" },
            { SystemLanguage.Japanese, "スタートをプレス ({0})" }
        };
    }

    private static Dictionary<SystemLanguage, string> KeyboardTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Teclado" },
            { SystemLanguage.English, "Keyboard" },
            { SystemLanguage.Portuguese, "Teclado" },
            { SystemLanguage.French, "Clavier" },
            { SystemLanguage.German, "Tastatur" },
            { SystemLanguage.Catalan, "Teclat" },
            { SystemLanguage.Basque, "Teklatua" },
            { SystemLanguage.Japanese, "キーボード" }
        };
    }

    private static Dictionary<SystemLanguage, string> GamepadTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Mando" },
            { SystemLanguage.English, "Gamepad" },
            { SystemLanguage.Portuguese, "Gamepad" },
            { SystemLanguage.French, "Gamepad" },
            { SystemLanguage.German, "Gamepad" },
            { SystemLanguage.Catalan, "Gamepad" },
            { SystemLanguage.Basque, "Gamepad" },
            { SystemLanguage.Japanese, "ゲームパッド" }
        };
    }

    private static Dictionary<SystemLanguage, string> JumpTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Saltar" },
            { SystemLanguage.English, "Jump" },
            { SystemLanguage.Portuguese, "Saltar" },
            { SystemLanguage.French, "Jump" },
            { SystemLanguage.German, "Jump" },
            { SystemLanguage.Catalan, "Saltar" },
            { SystemLanguage.Basque, "Saltatu" },
            { SystemLanguage.Japanese, "ジャンプ" }
        };
    }

    private static Dictionary<SystemLanguage, string> ShootTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Disparo" },
            { SystemLanguage.English, "Shoot" },
            { SystemLanguage.Portuguese, "Atirar" },
            { SystemLanguage.French, "Tirer" },
            { SystemLanguage.German, "Schießen" },
            { SystemLanguage.Catalan, "Tret" },
            { SystemLanguage.Basque, "Jaurti" },
            { SystemLanguage.Japanese, "シュート" }
        };
    }

    private static Dictionary<SystemLanguage, string> LookUpTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Arriba" },
            { SystemLanguage.English, "Up" },
            { SystemLanguage.Portuguese, "Acima" },
            { SystemLanguage.French, "Haut" },
            { SystemLanguage.German, "Oben" },
            { SystemLanguage.Catalan, "Adalt" },
            { SystemLanguage.Basque, "Gora" },
            { SystemLanguage.Japanese, "アップ" }
        };
    }

    private static Dictionary<SystemLanguage, string> CrouchTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Agacharse" },
            { SystemLanguage.English, "Crouch" },
            { SystemLanguage.Portuguese, "Saltar" },
            { SystemLanguage.French, "Jump" },
            { SystemLanguage.German, "Jump" },
            { SystemLanguage.Catalan, "Saltar" },
            { SystemLanguage.Basque, "Makurtu" },
            { SystemLanguage.Japanese, "しゃがむ" }
        };
    }

    private static Dictionary<SystemLanguage, string> MoveLeftTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Izquierda" },
            { SystemLanguage.English, "Move Left" },
            { SystemLanguage.Portuguese, "Esquerda" },
            { SystemLanguage.French, "Gauche" },
            { SystemLanguage.German, "Links" },
            { SystemLanguage.Catalan, "Esquerra" },
            { SystemLanguage.Basque, "Ezkerra" },
            { SystemLanguage.Japanese, "左" }
        };
    }

    private static Dictionary<SystemLanguage, string> MoveRightTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Derecha" },
            { SystemLanguage.English, "Move Right" },
            { SystemLanguage.Portuguese, "Direita" },
            { SystemLanguage.French, "Droite" },
            { SystemLanguage.German, "Recht" },
            { SystemLanguage.Catalan, "Dreta" },
            { SystemLanguage.Basque, "Eskuma" },
            { SystemLanguage.Japanese, "右" }
        };
    }

    private static Dictionary<SystemLanguage, string> LoadingTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Cargando" },
            { SystemLanguage.English, "Loading" },
            { SystemLanguage.Portuguese, "Carregando" },
            { SystemLanguage.French, "Chargement" },
            { SystemLanguage.German, "Beladung" },
            { SystemLanguage.Catalan, "Carregant" },
            { SystemLanguage.Basque, "Karga" },
            { SystemLanguage.Japanese, "ローディング" }
        };
    }

    // End act

    private static Dictionary<SystemLanguage, string> GameEndTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Juego completado" },
            { SystemLanguage.English, "Game completed" },
            { SystemLanguage.Portuguese, "Jogo completo" },
            { SystemLanguage.French, "Jeu complet" },
            { SystemLanguage.German, "Spiel abgeschlossen" },
            { SystemLanguage.Catalan, "Joc complet" },
            { SystemLanguage.Basque, "Jokoa osatu da" },
            { SystemLanguage.Japanese, "ゲーム完了" }
        };
    }

    private static Dictionary<SystemLanguage, string> ActEndTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Acto {0} completo" },
            { SystemLanguage.English, "Act {0} completed" },
            { SystemLanguage.Portuguese, "Ato {0} concluído" },
            { SystemLanguage.French, "Acte {0} terminét" },
            { SystemLanguage.German, "Akt {0} abgeschlossen" },
            { SystemLanguage.Catalan, "Acte {0} complet" },
            { SystemLanguage.Basque, "Ekintza {0} osatu" },
            { SystemLanguage.Japanese, "行為{0}完了" }
        };
    }

    private static Dictionary<SystemLanguage, string> TimeBonusTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Bonus por tiempo" },
            { SystemLanguage.English, "Time bonus" },
            { SystemLanguage.Portuguese, "Bônus de tempo" },
            { SystemLanguage.French, "Bonus de temps" },
            { SystemLanguage.German, "Zeit Bonus" },
            { SystemLanguage.Catalan, "Bonus per temps" },
            { SystemLanguage.Basque, "Denbora bonus" },
            { SystemLanguage.Japanese, "タイムボーナス" }
        };
    }

    private static Dictionary<SystemLanguage, string> ClearBonusTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Bonus eliminados" },
            { SystemLanguage.English, "Clear bonus" },
            { SystemLanguage.Portuguese, "Bônus eliminados" },
            { SystemLanguage.French, "Bonus éliminés" },
            { SystemLanguage.German, "Beseitigte Bonus" },
            { SystemLanguage.Catalan, "Bonus eliminats" },
            { SystemLanguage.Basque, "Hobariak ezabatuak" },
            { SystemLanguage.Japanese, "クリアボーナス" }
        };
    }

    private static Dictionary<SystemLanguage, string> WaferBonusTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Bonus obleas" },
            { SystemLanguage.English, "Wafer bonus" },
            { SystemLanguage.Portuguese, "Bônus de bolacha" },
            { SystemLanguage.French, "Bonus de tranches" },
            { SystemLanguage.German, "Wafer-Bonus" },
            { SystemLanguage.Catalan, "Bonus hòsties" },
            { SystemLanguage.Basque, "Olioaren bonus" },
            { SystemLanguage.Japanese, "ウェファボーナス" }
        };
    }

    private static Dictionary<SystemLanguage, string> MonasteryBossName() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Monja Nudista" },
            { SystemLanguage.English, "Nude Nun" },
            { SystemLanguage.Portuguese, "Freira Nudista" },
            { SystemLanguage.French, "Nonne Naturiste" },
            { SystemLanguage.German, "Nackte Nonne" },
            { SystemLanguage.Catalan, "Monja Nudista" },
            { SystemLanguage.Basque, "Monja Nudista" },
            { SystemLanguage.Japanese, "裸修道女" }
        };
    }

    private static Dictionary<SystemLanguage, string> MonasteryBossTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Dios nos creó con nuestros cuerpos, ¿por qué no deberíamos mostrarlos?\n¡Tu absurda cruzada acaba aquí, Monja!" },
            { SystemLanguage.English, "God created us with our bodies, so why should we not show them freely?\nYour foolish crusade ends here, Nun!" },
            { SystemLanguage.Portuguese, "Deus nos criou com nossos corpos, então por que não devemos mostrá-los livremente?\nSua cruzada tola termina aqui, Freira!" },
            { SystemLanguage.French, "Dieu nous a créés avec nos corps, alors pourquoi ne devrions-nous pas les montrer librement?\nVotre croisade insensée se termine ici, Religieuse!" },
            { SystemLanguage.German, "Gott erschuf uns mit unseren Körpern, warum sollten wir sie nicht frei zeigen?\n Dein irrer Kreuzzug endet hier, Nun!" },
            { SystemLanguage.Catalan, "Déu ens va crear amb els nostres cosos, per què no hauríem de mostrar-los?\nLa teva croada absurda acaba aquí, Monja!" },
            { SystemLanguage.Basque, "Jainkoak gure gorputzekin sortu gintuen, zergatik ez genituzke erakutsi beharko?\nZure gurutzada hemen amaitzen da, Monja!" },
            { SystemLanguage.Japanese, "神さまは私たちの体で私たちを造られました、だからなぜ私達はそれらを自由に見せてはいけませんか?\n手前の愚かな十字軍はここで終わります、修道女よ!" }
        };
    }

    private static Dictionary<SystemLanguage, string> NightbarBossName() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Reina de la Noche" },
            { SystemLanguage.English, "Night Queen" },
            { SystemLanguage.Portuguese, "Rainha da Noite" },
            { SystemLanguage.French, "Reine de Nuit" },
            { SystemLanguage.German, "Nacht Königin" },
            { SystemLanguage.Catalan, "Reina de la Nit" },
            { SystemLanguage.Basque, "Gaueko Erregina" },
            { SystemLanguage.Japanese, "夜の女王" }
        };
    }

    private static Dictionary<SystemLanguage, string> NightbarBossTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "¿Es quizá el designio de Dios el deber vestirse como la terrestre Iglesia cree? ¡No, yo digo!" },
            { SystemLanguage.English, "Is it perchance God's will that we should dress as the earthbound Church decrees? No, I say!" },
            { SystemLanguage.Portuguese, "É talvez a vontade de Deus que nos vestimos como a Igreja ligada à Terra decreta? Não, eu digo!" },
            { SystemLanguage.French, "Est-ce vraiment la volonté de Dieu que de s'habiller comme l'Église sur terre le veut? Je ne crois pas, non!" },
            { SystemLanguage.German, "Ist es Gottes Wille, dass wir uns kleiden mögen wie es die weltliche Kirche verordnet? Mitnichten sage ich!" },
            { SystemLanguage.Catalan, "Es potser el designi de Déu el deure vestir-se com l'església terrestre creu? Jo dic no!" },
            { SystemLanguage.Basque, "Jainkoaren deseinua da Eliza lurtarrak uste duen bezala janztea? Ez dut uste! Ezetz, nik diot!" },
            { SystemLanguage.Japanese, "教会が言うように僕らが服を着るべきというのは神さまの意志ですか?\n絶対いないよ!" }
        };
    }

    private static Dictionary<SystemLanguage, string> HospitalBossName() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Madre Abortista" },
            { SystemLanguage.English, "Abortist Mom" },
            { SystemLanguage.Portuguese, "Mãe Abortista" },
            { SystemLanguage.French, "Mère Abortiste" },
            { SystemLanguage.German, "Abtreibenmutter" },
            { SystemLanguage.Catalan, "Mare Abortista" },
            { SystemLanguage.Basque, "Ama Abortzalea" },
            { SystemLanguage.Japanese, "中絶する母親" }
        };
    }

    private static Dictionary<SystemLanguage, string> HospitalBossTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "¡Si Dios no quería abortos, quizá no debería haber creado los abortos naturales!" },
            { SystemLanguage.English, "If God disapproves of abortion, maybe he shouldn't have designed miscarriages!" },
            { SystemLanguage.Portuguese, "Se Deus não quisesse abortos, talvez ele não deveria ter criado os abortos espontâneos!" },
            { SystemLanguage.French, "Si Dieu ne voulait pas d'avortements, peut-être n'auraient-ils pas dû créer avortements naturels!" },
            { SystemLanguage.German, "Wenn Gott keine Abtreibungen möchte, vielleicht hätten sie keine natürlichen Abtreibungen erschaffen sollen!" },
            { SystemLanguage.Catalan, "Si Déu no volia avortaments, potser no hauria de haver creat els avortaments naturals!" },
            { SystemLanguage.Basque, "Jainkoak ez badu abortziorik nahi, zergatik sortu zituen abortzio naturalak?" },
            { SystemLanguage.Japanese, "もし神さまが中絶を望まなかったら、多分彼らは自然な中絶を起こしてはいけませんでした！" }
        };
    }

    private static Dictionary<SystemLanguage, string> DesertBossName() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Muhaidjinn" },
            { SystemLanguage.English, "Muhaidjinn" },
            { SystemLanguage.Portuguese, "Muhaidjinn" },
            { SystemLanguage.French, "Muhaidjinn" },
            { SystemLanguage.German, "Muhaidjinn" },
            { SystemLanguage.Catalan, "Muhaidjinn" },
            { SystemLanguage.Basque, "Muhaidjinn" },
            { SystemLanguage.Japanese, "ムハイジン" }
        };
    }

    private static Dictionary<SystemLanguage, string> DesertBossTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "¡He salvaguardado este lugar sagrado por miles de años—y pienso seguir haciéndolo!" },
            { SystemLanguage.English, "I've guarded this sacred place for thousands of years—and I intend to keep doing so!" },
            { SystemLanguage.Portuguese, "Eu guardei este lugar sagrado por milhares de anos—e pretendo continuar fazendo isso!" },
            { SystemLanguage.French, "Je garde cet endroit sacré depuis des milliers d'années—et j'ai l'intention de continuer à le faire!" },
            { SystemLanguage.German, "Ich bewachte diesen heiligen Ort seit tausenden Jahren-und ich beabsichtige dies weiter zu tun!" },
            { SystemLanguage.Catalan, "He vigilat aquest lloc sagrat per milers d'anys—i penso seguint fent-ho!" },
            { SystemLanguage.Basque, "Leku sakratu hau milaka urtez zaindu dut—eta gehiagoz segituko dut!" },
            { SystemLanguage.Japanese, "俺は何千年もの間この神聖な場所を守ってきました - そして、これからも守り続けるつもりです！" }
        };
    }

    private static Dictionary<SystemLanguage, string> LabBossName() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Científico" },
            { SystemLanguage.English, "Scientist" },
            { SystemLanguage.Portuguese, "Cientista" },
            { SystemLanguage.French, "Scientifique" },
            { SystemLanguage.German, "Wissenschaftler" },
            { SystemLanguage.Catalan, "Científic" },
            { SystemLanguage.Basque, "Zientzialaria" },
            { SystemLanguage.Japanese, "科学者" }
        };
    }

    private static Dictionary<SystemLanguage, string> LabBossTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "¡Las células madre están bien, hay que investigar clones y vas a ser antimaterializada!" },
            { SystemLanguage.English, "Stem cells are fine, clones should be researched, and you are about to get anti-mattered!" },
            { SystemLanguage.Portuguese, "As células-tronco estão bem, os clones devem ser pesquisados e você está prestes a desaparecer!" },
            { SystemLanguage.French, "Les cellules souches vont bien, il faut faire des recherches sur les clones et vous êtes sur le point de disparaître!" },
            { SystemLanguage.German, "Ich bewachte diesen heiligen Ort seit tausenden Jahren-und ich beabsichtige dies weiter zu tun!" },
            { SystemLanguage.Catalan, "Les cél·lules mare estàn bé, s'ha d'investigar la clonació i seràs antimaterialitzada!" },
            { SystemLanguage.Basque, "Zelula amak ondo daude, klonak ikertu behar dira eta antimaterializatua izango zara!" },
            { SystemLanguage.Japanese, "幹細胞は問題ありません、クローンは研究されるべきです、そして貴様はアンチマターデッドになります！" }
        };
    }

    private static Dictionary<SystemLanguage, string> HqBossName() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Josyf Potzedong" },
            { SystemLanguage.English, "Josyf Potzedong" },
            { SystemLanguage.Portuguese, "Josyf Potzedong" },
            { SystemLanguage.French, "Josyf Potzedong" },
            { SystemLanguage.German, "Josyf Potzedong" },
            { SystemLanguage.Catalan, "Josyf Potzedong" },
            { SystemLanguage.Basque, "Josyf Potzedong" },
            { SystemLanguage.Japanese, "ジオシフポツエドング" }
        };
    }

    private static Dictionary<SystemLanguage, string> CommieBossTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "¡Hemos vuelto para quedarnos!\n¡La plusvalía es del obrero que la produce!" },
            { SystemLanguage.English, "Wir sind zurück und wir bleiben hier!\nArbeiterarbeit ist ihre eigene!" },
            { SystemLanguage.Portuguese, "Estamos de volta e estamos aqui para permanecer!\nO trabalho dos trabalhadores é deles!" },
            { SystemLanguage.French, "Nous sommes de retour pour rester!\nLe travail des ouvriers est le leur!" },
            { SystemLanguage.German, "Wir sind zurück und beabsichtigen zu bleiben!!\nDes Arbeiters Lohn ist ihr eigener!" },
            { SystemLanguage.Catalan, "Hem tornat per quedar-nos!\nLa plusvàlua perteneix al obrer que la produeix!" },
            { SystemLanguage.Basque, "Gelditzeko itzuli gara!\nGainbalioa hau ekoizten duen langilearena da!" },
            { SystemLanguage.Japanese, "俺らは戻ってきましたそして俺らはここにいますよ！労働者の労働は彼ら自身のものですよ！" }
        };
    }

    private static Dictionary<SystemLanguage, string> PeterBossName() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Líder del PC" },
            { SystemLanguage.English, "CP Leader" },
            { SystemLanguage.Portuguese, "Líder do PC" },
            { SystemLanguage.French, "Secrétaire de PC" },
            { SystemLanguage.German, "KP Führer" },
            { SystemLanguage.Catalan, "Líder del PC" },
            { SystemLanguage.Basque, "PK-ren buruzagia" },
            { SystemLanguage.Japanese, "CP委員長" }
        };
    }

    private static Dictionary<SystemLanguage, string> PeterTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "¡No puedes detener al Partido Comunista!\n¡Hoy morirás, Monja!" },
            { SystemLanguage.English, "You cannot stop the Communist Party!\nToday you perish, Nun!" },
            { SystemLanguage.Portuguese, "Você não pode parar o Partido Comunista!\nHoje você perece, Freira!" },
            { SystemLanguage.French, "Vous ne pouvez pas arrêter le parti communiste!\nAujourd'hui tu péris, Nonne!" },
            { SystemLanguage.German, "Du kannst nicht die kommunistische Partei stoppen!\n Heute wirst du zugrunde gehen, Nonne!" },
            { SystemLanguage.Catalan, "No pots detenir al Partit Comunista!\nAvui moriràs, Monja!" },
            { SystemLanguage.Basque, "Ezin duzu Partidu Komunista geldiarazi!\nGaur hilko zara, Monja!" },
            { SystemLanguage.Japanese, "共産党を止めることはできません！\n今日は手前が死ぬ、修道女！" }
        };
    }

    private static Dictionary<SystemLanguage, string> LeninBossName() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Lenincara" },
            { SystemLanguage.English, "Leninface" },
            { SystemLanguage.Portuguese, "Lenincara" },
            { SystemLanguage.French, "Leninvisage" },
            { SystemLanguage.German, "Leningesicht" },
            { SystemLanguage.Catalan, "Lenincara" },
            { SystemLanguage.Basque, "Leninaurpegi" },
            { SystemLanguage.Japanese, "レニンの顔" }
        };
    }

    private static Dictionary<SystemLanguage, string> LeninfaceTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "¡Aplastar el capitalismo! ¡Groar!" },
            { SystemLanguage.English, "Smash capitalism! Growl!" },
            { SystemLanguage.Portuguese, "Esmagar o capitalismo! Groar!" },
            { SystemLanguage.French, "Smash le capitalisme! Groeur!" },
            { SystemLanguage.German, "Zerschlage Kapitalismus! Grrrr!" },
            { SystemLanguage.Catalan, "Aixafar el capitalisme! Groar!" },
            { SystemLanguage.Basque, "Kapitalismoa suntsitu! Groar!" },
            { SystemLanguage.Japanese, "資本主義を破りなさい！ おっらおっらおっら！" }
        };
    }

    private static Dictionary<SystemLanguage, string> MarxexBossName() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Marx-X" },
            { SystemLanguage.English, "Marx-X" },
            { SystemLanguage.Portuguese, "Marx-X" },
            { SystemLanguage.French, "Marx-X" },
            { SystemLanguage.German, "Marx-X" },
            { SystemLanguage.Catalan, "Marx-X" },
            { SystemLanguage.Basque, "Marx-X" },
            { SystemLanguage.Japanese, "マルクス-X" }
        };
    }

    private static Dictionary<SystemLanguage, string> MarxexTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "¡Estúpido ignorante de clase obrera!\n¡Acaba con este sinsentido!" },
            { SystemLanguage.English, "You working class ignorant fool!\nEnough of this nonsense!" },
            { SystemLanguage.Portuguese, "Você trabalha bobo ignorante!\nChega desse absurdo!" },
            { SystemLanguage.French, "Vous, imbécile ignorant de la classe ouvrière!\nAssez de ce non-sens!" },
            { SystemLanguage.German, "Du ignoranter arbeiterklassen Trottel!\nGenug von diesem Unsinn!" },
            { SystemLanguage.Catalan, "Tros d'ase de clase obrera! Acava amb aquesta bojeria!" },
            { SystemLanguage.Basque, "Klase langileko ezjakin alua!\nAmaitu zenztugabekeria honekin!" },
            { SystemLanguage.Japanese, "貴様は労働者階級無知なばか！このナンセンスで十分です！" }
        };
    }

    private static Dictionary<SystemLanguage, string> JesusBossName() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Jesús de Nazareth" },
            { SystemLanguage.English, "Jesus Christ" },
            { SystemLanguage.Portuguese, "Jesus de Nazaré" },
            { SystemLanguage.French, "Jesus de Nazaret" },
            { SystemLanguage.German, "Jesus von Nazareth" },
            { SystemLanguage.Catalan, "Jesús de Nazareth" },
            { SystemLanguage.Basque, "Jesus Nazaretekoa" },
            { SystemLanguage.Japanese, "イエス・キリスト" }
        };
    }

    private static Dictionary<SystemLanguage, string> JesusTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "¡Fui yo, siempre he sido yo! ¡Soy el primer comunista! ¡¿Crees que puedes pararme a MÍ, Monja?!" },
            { SystemLanguage.English, "Foi eu, sempre fui eu! Eu fui o primeiro comunista! Você acha que pode me parar, Freira?!" },
            { SystemLanguage.Portuguese, "It was me, it's always been me! I was the first communist! Do you think you can stop ME, Nun?!" },
            { SystemLanguage.French, "C'était moi, ça a toujours été moi! J'étais le premier communiste! Pensez-vous que vous pouvez m'arrêter, Nonne?!" },
            { SystemLanguage.German, "Das war ich, das war immer ich! Ich war der erste Kommunist! Glaubst du, du kannst MICH stoppen, Nonne?!" },
            { SystemLanguage.Catalan, "Era jo, sempre he sigut jo! Sóc el primer comunista! Creus que pots parar-me a MÍ, Monja?!" },
            { SystemLanguage.Basque, "Ni izan naiz, hasieratik ni izan naiz! Lehen komunista naiz! NI geldiarazteko gai zarela uste duzu, Monja?!" },
            { SystemLanguage.Japanese, "それは私でした、それはいつも私でした！私は最初の共産主義者でした！貴様は私、修道女をやめることができると思いますか？！" }
        };
    }

    private static Dictionary<SystemLanguage, string> HoldTexts() {
        return new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.Spanish, "Mantener" },
            { SystemLanguage.English, "Hold" },
            { SystemLanguage.Portuguese, "Manter" },
            { SystemLanguage.French, "Maintenir" },
            { SystemLanguage.German, "Pflegen" },
            { SystemLanguage.Catalan, "Mantenir" },
            { SystemLanguage.Basque, "Mantentzeko" },
            { SystemLanguage.Japanese, "保つ" }
        };
    }
}
