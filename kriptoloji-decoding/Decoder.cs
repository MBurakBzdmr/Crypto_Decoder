using System.Globalization;
using System.Numerics;
using System.Text;

namespace kriptoloji_decoding
{
    public partial class Decoder : Form
    {
        private static readonly string TurkAlfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

        private static readonly Color SidebarBg = Color.FromArgb(25, 42, 62);
        private static readonly Color SidebarBtnNormal = Color.FromArgb(33, 55, 80);
        private static readonly Color SidebarBtnActive = Color.FromArgb(0, 150, 136);
        private static readonly Color ContentBg = Color.FromArgb(240, 242, 245);
        private static readonly Color CardBg = Color.White;
        private static readonly Color AccentColor = Color.FromArgb(0, 150, 136);

        private int _activeMethod = -1;
        private readonly Button[] _sidebarButtons = new Button[11];
        private readonly Panel[] _paramPanels = new Panel[11];

        private Label _lblMethodTitle = null!;
        private Label _lblMethodDesc = null!;
        private TextBox _txtInput = null!;
        private TextBox _txtOutput = null!;
        private Panel _panelKeyArea = null!;

        private NumericUpDown _nudShiftKey = null!;
        private NumericUpDown _nudAffineA = null!;
        private NumericUpDown _nudAffineB = null!;
        private TextBox _txtSubAlphabet = null!;
        private NumericUpDown _nudColCount = null!;
        private NumericUpDown _nudPermKey = null!;
        private TextBox _txtPermKey = null!;
        private NumericUpDown _nudRouteCols = null!;
        private NumericUpDown _nudRails = null!;
        private TextBox _txtVigenereKey = null!;
        private NumericUpDown _nudHillSize = null!;
        private TextBox _txtHillMatrix = null!;
        private NumericUpDown _nudRsaPrimeP = null!;
        private NumericUpDown _nudRsaPrimeQ = null!;
        private TextBox _txtRsaPublicExponentE = null!;

        private static readonly string[] MethodNames =
        {
            "Kaydırmalı Şifreleme",
            "Doğrusal Şifreleme",
            "Yer Değiştirme Şifreleme",
            "Sayı Anahtarlı Yer Değiştirme",
            "Permütasyon Şifreleme",
            "Rota Şifreleme",
            "Zigzag Şifreleme",
            "Dört Kare Şifreleme",
            "Vigenere Şifreleme",
            "Hill Şifreleme",
            "RSA Şifre Çözme"
        };

        private static readonly string[] MethodDescriptions =
        {
            "Her harf, anahtar değeri kadar geri kaydırılarak çözülür.  D(c) = (c − k) mod 29",
            "D(x) = a⁻¹ · (x − b) mod 29 formülüyle çözülür.",
            "Şifreli metindeki her harf, 29 harflik karma alfabedeki konumuna göre orijinal harfe dönüştürülür.",
            "Şifreli metin matrise sütun sütun yazılıp satır satır okunarak çözülür (Sütun Transpozisyonu).",
            "Metin bloklara ayrılır ve ters permütasyon uygulanarak orijinal sıra elde edilir.",
            "Şifreli metin spiral (girdap) sırayla matrise yerleştirilip satır satır okunarak çözülür.",
            "Şifreli metin zigzag düzeninde raylara dağıtılıp sırayla okunarak çözülür.",
            "İkişerli harf blokları (digram), dört 6×5 matris kullanılarak ters koordinat eşleşmesiyle çözülür.",
            "Her harf, anahtar kelimedeki sıradaki harfin değeri kadar geri kaydırılarak çözülür.  D(c) = (c − k) mod 29",
            "Şifreli metin n'li bloklara ayrılıp anahtar matrisinin mod 29 tersi ile çarpılarak çözülür.  P = K⁻¹·C mod 29",
            "Şifreli bloklar (sayılar) özel anahtar d ile M = C^d (mod n) ile çözülür; n = p·q, φ(n) = (p−1)(q−1), d·e ≡ 1 (mod φ)."
        };

        public Decoder()
        {
            InitializeComponent();
            BuildUI();
            SelectMethod(0);
        }

        // ──────────────────────────────────────────────
        //  UI OLUŞTURMA
        // ──────────────────────────────────────────────

        private void BuildUI()
        {
            BackColor = ContentBg;

            var sidebar = BuildSidebar();
            var content = BuildContentArea();

            Controls.Add(content);
            Controls.Add(sidebar);
        }

        private Panel BuildSidebar()
        {
            var sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = SidebarBg
            };

            var lblLogo = new Label
            {
                Text = "\U0001F510  KRİPTOLOJİ",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 18),
                Size = new Size(230, 32),
                TextAlign = ContentAlignment.MiddleCenter
            };
            sidebar.Controls.Add(lblLogo);

            var lblSub = new Label
            {
                Text = "Şifre Çözücü",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(140, 180, 200),
                Location = new Point(10, 50),
                Size = new Size(230, 22),
                TextAlign = ContentAlignment.MiddleCenter
            };
            sidebar.Controls.Add(lblSub);

            var sep = new Panel
            {
                BackColor = Color.FromArgb(55, 75, 100),
                Location = new Point(20, 82),
                Size = new Size(210, 1)
            };
            sidebar.Controls.Add(sep);

            string[] btnLabels =
            {
                "  1.  Kaydırmalı",
                "  2.  Doğrusal",
                "  3.  Yer Değiştirme",
                "  4.  Sayı Anahtarlı",
                "  5.  Permütasyon",
                "  6.  Rota",
                "  7.  Zigzag",
                "  8.  Dört Kare",
                "  9.  Vigenere",
                "  10. Hill",
                "  11. RSA"
            };

            for (int i = 0; i < 11; i++)
            {
                int idx = i;
                var btn = new Button
                {
                    Text = btnLabels[i],
                    FlatStyle = FlatStyle.Flat,
                    BackColor = SidebarBtnNormal,
                    ForeColor = Color.FromArgb(200, 215, 230),
                    Font = new Font("Segoe UI", 10f),
                    Size = new Size(220, 40),
                    Location = new Point(15, 96 + i * 46),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 70, 100);
                btn.Click += (_, _) => SelectMethod(idx);

                _sidebarButtons[i] = btn;
                sidebar.Controls.Add(btn);
            }

            return sidebar;
        }

        private Panel BuildContentArea()
        {
            // Sidebar 250px, ClientSize 1100x750 → content alanı 850x750.
            // Anchor hesaplamaları doğru çalışsın diye panel boyutu dock öncesi ayarlanır.
            var content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ContentBg,
                Size = new Size(850, 750)
            };

            _lblMethodTitle = new Label
            {
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(30, 15),
                Size = new Size(790, 36),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            content.Controls.Add(_lblMethodTitle);

            _lblMethodDesc = new Label
            {
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(30, 52),
                Size = new Size(790, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            content.Controls.Add(_lblMethodDesc);

            content.Controls.Add(MakeSectionLabel("Şifreli Metin", 30, 90));

            _txtInput = new TextBox
            {
                Multiline = true,
                Font = new Font("Consolas", 11f),
                BackColor = CardBg,
                ForeColor = Color.FromArgb(33, 37, 41),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(30, 115),
                Size = new Size(790, 85),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ScrollBars = ScrollBars.Vertical
            };
            content.Controls.Add(_txtInput);

            content.Controls.Add(MakeSectionLabel("Anahtar Parametreleri", 30, 215));

            _panelKeyArea = new Panel
            {
                Location = new Point(30, 240),
                Size = new Size(790, 70),
                BackColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            content.Controls.Add(_panelKeyArea);

            CreateParamPanels();

            var btnCoz = new Button
            {
                Text = "Ç  Ö  Z",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                BackColor = AccentColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 44),
                Location = new Point(30, 326),
                Cursor = Cursors.Hand
            };
            btnCoz.FlatAppearance.BorderSize = 0;
            btnCoz.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 121, 107);
            btnCoz.Click += BtnCoz_Click;
            content.Controls.Add(btnCoz);

            var btnTemizle = new Button
            {
                Text = "Temizle",
                Font = new Font("Segoe UI", 10f),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 44),
                Location = new Point(180, 326),
                Cursor = Cursors.Hand
            };
            btnTemizle.FlatAppearance.BorderSize = 0;
            btnTemizle.FlatAppearance.MouseOverBackColor = Color.FromArgb(90, 98, 104);
            btnTemizle.Click += (_, _) => { _txtInput.Clear(); _txtOutput.Clear(); };
            content.Controls.Add(btnTemizle);

            content.Controls.Add(MakeSectionLabel("Çözülmüş Metin", 30, 386));

            _txtOutput = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Consolas", 11f),
                BackColor = Color.FromArgb(232, 245, 233),
                ForeColor = Color.FromArgb(27, 94, 32),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(30, 411),
                Size = new Size(790, 290),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ScrollBars = ScrollBars.Vertical
            };
            content.Controls.Add(_txtOutput);

            return content;
        }

        private void CreateParamPanels()
        {
            for (int i = 0; i < 11; i++)
            {
                _paramPanels[i] = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Transparent,
                    Visible = false
                };
                _panelKeyArea.Controls.Add(_paramPanels[i]);
            }

            // 0 – Kaydırmalı
            _nudShiftKey = MakeNumericUpDown(195, 14, 0, 28, 3);
            _paramPanels[0].Controls.Add(MakeParamLabel("Kaydırma Değeri (k):", 12, 16));
            _paramPanels[0].Controls.Add(_nudShiftKey);

            // 1 – Doğrusal (Affine)
            _nudAffineA = MakeNumericUpDown(100, 14, 1, 28, 1);
            _nudAffineB = MakeNumericUpDown(290, 14, 0, 28, 0);
            _paramPanels[1].Controls.Add(MakeParamLabel("a değeri:", 12, 16));
            _paramPanels[1].Controls.Add(_nudAffineA);
            _paramPanels[1].Controls.Add(MakeParamLabel("b değeri:", 200, 16));
            _paramPanels[1].Controls.Add(_nudAffineB);

            // 2 – Yer Değiştirme
            _txtSubAlphabet = new TextBox
            {
                Font = new Font("Consolas", 10f),
                Location = new Point(140, 14),
                Size = new Size(575, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                CharacterCasing = CharacterCasing.Upper
            };
            _paramPanels[2].Controls.Add(MakeParamLabel("Şifre Alfabesi:", 12, 16));
            _paramPanels[2].Controls.Add(_txtSubAlphabet);
            _paramPanels[2].Controls.Add(MakeHintLabel("29 harflik karma Türk alfabesi girin (ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ sıralaması ile)", 12, 44));

            // 3 – Sayı Anahtarlı Yer Değiştirme (Sütun Transpozisyonu)
            _nudColCount = MakeNumericUpDown(140, 14, 2, 50, 4);
            _paramPanels[3].Controls.Add(MakeParamLabel("Sütun Sayısı:", 12, 16));
            _paramPanels[3].Controls.Add(_nudColCount);
            _paramPanels[3].Controls.Add(MakeHintLabel("Şifrelemede kullanılan sütun sayısını (Anahtar-1) girin", 12, 44));

            // 4 – Permütasyon
            _nudPermKey = MakeNumericUpDown(100, 14, 2, 20, 5);
            _txtPermKey = new TextBox
            {
                Font = new Font("Consolas", 10f),
                Location = new Point(340, 14),
                Size = new Size(300, 25)
            };
            _paramPanels[4].Controls.Add(MakeParamLabel("Anahtar:", 12, 16));
            _paramPanels[4].Controls.Add(_nudPermKey);
            _paramPanels[4].Controls.Add(MakeParamLabel("Anahtar Sırası:", 205, 16));
            _paramPanels[4].Controls.Add(_txtPermKey);
            _paramPanels[4].Controls.Add(MakeHintLabel("Anahtar: blok boyutu — Sıra: Örn: 21453 veya 2 1 4 5 3 (boşluklu da olur)", 12, 44));

            // 5 – Rota
            _nudRouteCols = MakeNumericUpDown(140, 14, 2, 50, 4);
            _paramPanels[5].Controls.Add(MakeParamLabel("Sütun Sayısı:", 12, 16));
            _paramPanels[5].Controls.Add(_nudRouteCols);

            // 6 – Zigzag
            _nudRails = MakeNumericUpDown(175, 14, 2, 20, 3);
            _paramPanels[6].Controls.Add(MakeParamLabel("Satır (Ray) Sayısı:", 12, 16));
            _paramPanels[6].Controls.Add(_nudRails);

            // 7 – Dört Kare
            _paramPanels[7].Controls.Add(MakeParamLabel("Sabit karışık alfabe kullanılır — ek parametre gerekmez.", 12, 16));
            _paramPanels[7].Controls.Add(MakeHintLabel("Standart: ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZX  |  Karışık: YÖPÇİXŞRĞJÜZKCVINUFMHGSDEABOTL", 12, 44));

            // 8 – Vigenere
            _txtVigenereKey = new TextBox
            {
                Font = new Font("Consolas", 10f),
                Location = new Point(160, 14),
                Size = new Size(280, 25),
                CharacterCasing = CharacterCasing.Upper
            };
            _paramPanels[8].Controls.Add(MakeParamLabel("Anahtar Kelime:", 12, 16));
            _paramPanels[8].Controls.Add(_txtVigenereKey);
            _paramPanels[8].Controls.Add(MakeHintLabel("Örn: ANAHTAR — Her harf sırayla kaydırma değeri olarak kullanılır", 12, 44));

            // 9 – Hill
            _nudHillSize = MakeNumericUpDown(120, 14, 2, 5, 2);
            _txtHillMatrix = new TextBox
            {
                Font = new Font("Consolas", 10f),
                Location = new Point(340, 14),
                Size = new Size(370, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            _paramPanels[9].Controls.Add(MakeParamLabel("Matris Boyutu:", 12, 16));
            _paramPanels[9].Controls.Add(_nudHillSize);
            _paramPanels[9].Controls.Add(MakeParamLabel("Matris (satır satır):", 210, 16));
            _paramPanels[9].Controls.Add(_txtHillMatrix);
            _paramPanels[9].Controls.Add(MakeHintLabel("Boşlukla ayırın. 2×2 örnek: 7 8 11 23  |  3×3 örnek: 2 4 5 9 2 1 3 17 7  |  Dolgu harfi: A", 12, 44));

            // 10 – RSA şifre çözme
            _nudRsaPrimeP = MakeNumericUpDown(100, 14, 2, 9999, 19);
            _nudRsaPrimeQ = MakeNumericUpDown(265, 14, 2, 9999, 41);
            _txtRsaPublicExponentE = new TextBox
            {
                Font = new Font("Consolas", 10f),
                Location = new Point(430, 14),
                Size = new Size(120, 25),
                Text = "7"
            };
            _paramPanels[10].Controls.Add(MakeParamLabel("1. Asal (p):", 12, 16));
            _paramPanels[10].Controls.Add(_nudRsaPrimeP);
            _paramPanels[10].Controls.Add(MakeParamLabel("2. Asal (q):", 180, 16));
            _paramPanels[10].Controls.Add(_nudRsaPrimeQ);
            _paramPanels[10].Controls.Add(MakeParamLabel("Açık üs (e):", 348, 16));
            _paramPanels[10].Controls.Add(_txtRsaPublicExponentE);
            _paramPanels[10].Controls.Add(MakeHintLabel("Şifreli metin: blokları boşlukla ayırın (örn. 1234 5678). Her blok bir Unicode karaktere eşlenir.", 12, 44));
        }

        private void SelectMethod(int index)
        {
            _activeMethod = index;

            for (int i = 0; i < 11; i++)
            {
                bool active = i == index;
                _sidebarButtons[i].BackColor = active ? SidebarBtnActive : SidebarBtnNormal;
                _sidebarButtons[i].ForeColor = active ? Color.White : Color.FromArgb(200, 215, 230);
                _sidebarButtons[i].Font = new Font("Segoe UI", 10f, active ? FontStyle.Bold : FontStyle.Regular);
                _paramPanels[i].Visible = active;
            }

            _lblMethodTitle.Text = MethodNames[index];
            _lblMethodDesc.Text = MethodDescriptions[index];
        }

        // ──────────────────────────────────────────────
        //  ÇÖZ BUTONU
        // ──────────────────────────────────────────────

        private void BtnCoz_Click(object? sender, EventArgs e)
        {
            string input = _txtInput.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                ShowWarning("Lütfen şifreli metni girin.");
                return;
            }

            // RSA girdisi sayı bloklarıdır; klasik şifrelere Türkçe büyük harf normalizasyonu uygulanır.
            if (_activeMethod != 10)
                input = input.Replace("i", "İ").ToUpper();

            try
            {
                string result = _activeMethod switch
                {
                    0 => DecodeShift(input, (int)_nudShiftKey.Value),
                    1 => DecodeAffine(input, (int)_nudAffineA.Value, (int)_nudAffineB.Value),
                    2 => DecodeSubstitution(input, _txtSubAlphabet.Text.Trim().Replace("i", "İ").ToUpper()),
                    3 => DecodeColumnar(input, (int)_nudColCount.Value),
                    4 => DecodePermutation(input, (int)_nudPermKey.Value, _txtPermKey.Text.Trim()),
                    5 => DecodeRoute(input, (int)_nudRouteCols.Value),
                    6 => DecodeZigzag(input, (int)_nudRails.Value),
                    7 => DecodeFourSquare(input),
                    8 => DecodeVigenere(input, _txtVigenereKey.Text.Trim()),
                    9 => DecodeHill(input, (int)_nudHillSize.Value, _txtHillMatrix.Text.Trim()),
                    10 => DecodeRsa(input, (int)_nudRsaPrimeP.Value, (int)_nudRsaPrimeQ.Value, _txtRsaPublicExponentE.Text),
                    _ => string.Empty
                };

                _txtOutput.Text = result;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        // ──────────────────────────────────────────────
        //  1. KAYDIRMALI ŞİFRELEME (Caesar / Shift)
        //  Encode: C(p) = (p + k) mod 29
        //  Decode: D(c) = (c − k) mod 29
        // ──────────────────────────────────────────────

        private static string DecodeShift(string cipher, int key)
        {
            var sb = new StringBuilder(cipher.Length);
            foreach (char c in cipher)
            {
                int index = TurkAlfabe.IndexOf(c);
                if (index != -1)
                {
                    int yeniIndex = ((index - key) % 29 + 29) % 29;
                    sb.Append(TurkAlfabe[yeniIndex]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        // ──────────────────────────────────────────────
        //  2. DOĞRUSAL ŞİFRELEME (Affine)
        //  Encode: C(p) = (a·p + b) mod 29
        //  Decode: D(c) = a⁻¹·(c − b) mod 29
        // ──────────────────────────────────────────────

        private static string DecodeAffine(string cipher, int a, int b)
        {
            int aInv = ModInverse(a, 29);
            if (aInv == -1)
                throw new InvalidOperationException(
                    $"a={a} değerinin 29 ile aralarında asal olması gerekir.\n29 asal olduğundan 1-28 arası tüm değerler geçerlidir.");

            var sb = new StringBuilder(cipher.Length);
            foreach (char c in cipher)
            {
                int index = TurkAlfabe.IndexOf(c);
                if (index != -1)
                {
                    int diff = ((index - b) % 29 + 29) % 29;
                    int plain = (aInv * diff) % 29;
                    sb.Append(TurkAlfabe[plain]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static int ModInverse(int a, int m)
        {
            a = ((a % m) + m) % m;
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1)
                    return x;
            }
            return -1;
        }

        // ──────────────────────────────────────────────
        //  3. YER DEĞİŞTİRME ŞİFRELEME (Substitution)
        //  Encode: standartAlfabe[i] → karmaAlfabe[i]
        //  Decode: karmaAlfabe[i] → standartAlfabe[i]
        // ──────────────────────────────────────────────

        private static string DecodeSubstitution(string cipher, string karmaAlfabe)
        {
            if (karmaAlfabe.Length != 29)
                throw new InvalidOperationException("Şifre alfabesi tam 29 harf içermelidir!");

            var sb = new StringBuilder(cipher.Length);
            foreach (char c in cipher)
            {
                int index = karmaAlfabe.IndexOf(c);
                if (index != -1)
                {
                    sb.Append(TurkAlfabe[index]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        // ──────────────────────────────────────────────
        //  4. SAYI ANAHTARLI YER DEĞİŞTİRME
        //  Encode: Matrise satır satır yaz, sütun sütun oku
        //  Decode: Matrise sütun sütun yaz, satır satır oku
        // ──────────────────────────────────────────────

        private static string DecodeColumnar(string cipher, int sutunSayisi)
        {
            if (sutunSayisi <= 0)
                throw new InvalidOperationException("Sütun sayısı 0'dan büyük olmalıdır!");

            string islenecekMetin = cipher.Replace(" ", "");
            int uzunluk = islenecekMetin.Length;
            int satirSayisi = (int)Math.Ceiling((double)uzunluk / sutunSayisi);

            char[,] matris = new char[satirSayisi, sutunSayisi];
            int harfIndex = 0;

            for (int j = 0; j < sutunSayisi; j++)
            {
                for (int i = 0; i < satirSayisi; i++)
                {
                    if (harfIndex < uzunluk)
                    {
                        matris[i, j] = islenecekMetin[harfIndex];
                        harfIndex++;
                    }
                }
            }

            var sb = new StringBuilder(uzunluk);
            for (int i = 0; i < satirSayisi; i++)
            {
                for (int j = 0; j < sutunSayisi; j++)
                {
                    if (matris[i, j] != '\0')
                        sb.Append(matris[i, j]);
                }
            }

            return sb.ToString();
        }

        // ──────────────────────────────────────────────
        //  5. PERMÜTASYON ŞİFRELEME
        //  Encode: cipher[i] = block[perm[i] − 1]
        //  Decode: block[perm[i] − 1] = cipher[i]
        // ──────────────────────────────────────────────

        private static string DecodePermutation(string cipher, int key, string permStr)
        {
            int[] perm = ParsePermutation(permStr);
            if (perm.Length == 0)
                throw new InvalidOperationException("Lütfen geçerli bir permütasyon sırası girin!\nÖrn: 21453 veya 2 1 4 5 3");

            if (perm.Length != key)
                throw new InvalidOperationException(
                    $"Anahtar sırası {perm.Length} elemanlı, ancak anahtar {key} olarak girilmiş.\n" +
                    $"Anahtar sırası tam olarak {key} eleman içermelidir.");

            var sorted = perm.OrderBy(x => x).ToArray();
            for (int i = 0; i < key; i++)
            {
                if (sorted[i] != i + 1)
                    throw new InvalidOperationException(
                        $"Anahtar sırası 1'den {key}'e kadar olan sayıları tam olarak birer kez içermelidir.");
            }

            string islenecekMetin = cipher.Replace(" ", "");
            int totalLen = (int)Math.Ceiling((double)islenecekMetin.Length / key) * key;
            string padded = islenecekMetin.PadRight(totalLen);

            var result = new char[totalLen];
            for (int b = 0; b < totalLen / key; b++)
            {
                int offset = b * key;
                for (int i = 0; i < key; i++)
                    result[offset + perm[i] - 1] = padded[offset + i];
            }

            return new string(result).TrimEnd();
        }

        private static int[] ParsePermutation(string s)
        {
            s = s.Trim();
            if (string.IsNullOrEmpty(s)) return Array.Empty<int>();

            if (s.Contains(' '))
            {
                return s.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();
            }
            return s.Where(char.IsDigit).Select(ch => ch - '0').ToArray();
        }

        // ──────────────────────────────────────────────
        //  6. ROTA ŞİFRELEME (Spiral Transpozisyon)
        //  Encode: Matrise satır satır yaz, spiral sırayla oku
        //  Decode: Matrise spiral sırayla yaz, satır satır oku
        // ──────────────────────────────────────────────

        private static string DecodeRoute(string cipher, int sutunSayisi)
        {
            if (sutunSayisi <= 0)
                throw new InvalidOperationException("Sütun sayısı 0'dan büyük olmalıdır!");

            string islenecekMetin = cipher.Replace(" ", "");
            int uzunluk = islenecekMetin.Length;
            int satirSayisi = (int)Math.Ceiling((double)uzunluk / sutunSayisi);

            int matrisSize = satirSayisi * sutunSayisi;
            if (islenecekMetin.Length < matrisSize)
                islenecekMetin = islenecekMetin.PadRight(matrisSize, 'R');

            char[,] matris = new char[satirSayisi, sutunSayisi];

            var positions = new List<(int row, int col)>();
            int ustSinir = 0, altSinir = satirSayisi - 1;
            int solSinir = 0, sagSinir = sutunSayisi - 1;

            while (ustSinir <= altSinir && solSinir <= sagSinir)
            {
                for (int i = altSinir; i >= ustSinir; i--)
                    positions.Add((i, solSinir));
                solSinir++;

                if (solSinir > sagSinir) break;

                for (int j = solSinir; j <= sagSinir; j++)
                    positions.Add((ustSinir, j));
                ustSinir++;

                if (ustSinir > altSinir) break;

                for (int i = ustSinir; i <= altSinir; i++)
                    positions.Add((i, sagSinir));
                sagSinir--;

                if (solSinir > sagSinir) break;

                for (int j = sagSinir; j >= solSinir; j--)
                    positions.Add((altSinir, j));
                altSinir--;
            }

            for (int k = 0; k < positions.Count && k < islenecekMetin.Length; k++)
            {
                var (r, c) = positions[k];
                matris[r, c] = islenecekMetin[k];
            }

            var sb = new StringBuilder(uzunluk);
            for (int i = 0; i < satirSayisi; i++)
                for (int j = 0; j < sutunSayisi; j++)
                    if (matris[i, j] != '\0')
                        sb.Append(matris[i, j]);

            return sb.ToString();
        }

        // ──────────────────────────────────────────────
        //  7. ZİGZAG ŞİFRELEME (Rail Fence)
        //  Encode: Harfler zigzag dağıtılır, satırlar birleştirilir
        //  Decode: Satır uzunlukları hesaplanıp şifreli metin raylara paylaştırılır
        // ──────────────────────────────────────────────

        private static string DecodeZigzag(string cipher, int rails)
        {
            if (rails <= 1) return cipher;
            int len = cipher.Length;

            var railLens = new int[rails];
            int rail = 0, dir = 1;
            for (int i = 0; i < len; i++)
            {
                railLens[rail]++;
                if (rail == 0) dir = 1;
                else if (rail == rails - 1) dir = -1;
                rail += dir;
            }

            var railTexts = new string[rails];
            int offset = 0;
            for (int r = 0; r < rails; r++)
            {
                railTexts[r] = cipher.Substring(offset, railLens[r]);
                offset += railLens[r];
            }

            var railIdx = new int[rails];
            rail = 0; dir = 1;
            var result = new char[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = railTexts[rail][railIdx[rail]++];
                if (rail == 0) dir = 1;
                else if (rail == rails - 1) dir = -1;
                rail += dir;
            }

            return new string(result);
        }

        // ──────────────────────────────────────────────
        //  8. DÖRT KARE ŞİFRELEME (Four Square)
        //  Encode: p1(solÜst satır, sütun) + p2(sağAlt satır, sütun) → c1=sagÜst[p1Satır, p2Sütun], c2=solAlt[p2Satır, p1Sütun]
        //  Decode: c1(sağÜst satır, sütun) + c2(solAlt satır, sütun) → p1=solÜst[c1Satır, c2Sütun], p2=sağAlt[c2Satır, c1Sütun]
        // ──────────────────────────────────────────────

        private static string DecodeFourSquare(string cipher)
        {
            const string standartAlfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZX";
            const string sabitKarisikAlfabe = "YÖPÇİXŞRĞJÜZKCVINUFMHGSDEABOTL";
            const int cols = 5;

            string solUst = standartAlfabe;
            string sagAlt = standartAlfabe;
            string sagUst = sabitKarisikAlfabe;
            string solAlt = sabitKarisikAlfabe;

            string islenecekMetin = cipher.Replace(" ", "");
            if (islenecekMetin.Length % 2 != 0)
                islenecekMetin += "X";

            var sb = new StringBuilder(islenecekMetin.Length);

            for (int i = 0; i < islenecekMetin.Length; i += 2)
            {
                char c1 = islenecekMetin[i];
                char c2 = islenecekMetin[i + 1];

                int c1Index = sagUst.IndexOf(c1);
                int c1Satir = c1Index / cols;
                int c1Sutun = c1Index % cols;

                int c2Index = solAlt.IndexOf(c2);
                int c2Satir = c2Index / cols;
                int c2Sutun = c2Index % cols;

                int p1Index = (c1Satir * cols) + c2Sutun;
                int p2Index = (c2Satir * cols) + c1Sutun;

                sb.Append(solUst[p1Index]);
                sb.Append(sagAlt[p2Index]);
            }

            return sb.ToString();
        }

        // ──────────────────────────────────────────────
        //  9. VIGENERE ŞİFRELEME
        //  Encode: C(i) = (P(i) + K(i)) mod 29
        //  Decode: P(i) = (C(i) − K(i)) mod 29
        // ──────────────────────────────────────────────

        private static string DecodeVigenere(string cipher, string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("Lütfen bir anahtar kelime girin!");

            key = key.Replace("i", "İ").ToUpper();

            foreach (char k in key)
            {
                if (TurkAlfabe.IndexOf(k) == -1)
                    throw new InvalidOperationException($"Anahtar kelimede geçersiz karakter: '{k}'");
            }

            var sb = new StringBuilder(cipher.Length);
            int ki = 0;
            foreach (char c in cipher)
            {
                int cIndex = TurkAlfabe.IndexOf(c);
                if (cIndex != -1)
                {
                    int kIndex = TurkAlfabe.IndexOf(key[ki % key.Length]);
                    int plainIndex = ((cIndex - kIndex) % 29 + 29) % 29;
                    sb.Append(TurkAlfabe[plainIndex]);
                    ki++;
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        // ──────────────────────────────────────────────
        //  10. HILL ŞİFRELEME
        //  Encode: C = P · K mod 29  (satır vektörü × matris)
        //  Decode: P = C · K⁻¹ mod 29
        //  Dolgu harfi: A (indeks 0)
        // ──────────────────────────────────────────────

        private static string DecodeHill(string cipher, int n, string matrixStr)
        {
            int[,] keyMatrix = ParseHillMatrix(matrixStr, n);
            int[,] invMatrix = MatrixInverseMod29(keyMatrix, n);

            string islenecekMetin = cipher.Replace(" ", "");

            int eksik = islenecekMetin.Length % n;
            if (eksik != 0)
                islenecekMetin = islenecekMetin.PadRight(islenecekMetin.Length + (n - eksik), 'A');

            var sb = new StringBuilder(islenecekMetin.Length);

            for (int blok = 0; blok < islenecekMetin.Length; blok += n)
            {
                int[] cipherBlock = new int[n];
                for (int i = 0; i < n; i++)
                {
                    int idx = TurkAlfabe.IndexOf(islenecekMetin[blok + i]);
                    if (idx == -1)
                        throw new InvalidOperationException($"Geçersiz karakter: '{islenecekMetin[blok + i]}'");
                    cipherBlock[i] = idx;
                }

                // P = C · K⁻¹ (satır vektörü × ters matris)
                for (int col = 0; col < n; col++)
                {
                    int sum = 0;
                    for (int row = 0; row < n; row++)
                        sum += cipherBlock[row] * invMatrix[row, col];
                    sb.Append(TurkAlfabe[((sum % 29) + 29) % 29]);
                }
            }

            return sb.ToString();
        }

        private static int[,] ParseHillMatrix(string matrixStr, int n)
        {
            var values = matrixStr.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(int.Parse)
                                  .ToArray();

            if (values.Length != n * n)
                throw new InvalidOperationException(
                    $"Matris {n}×{n} = {n * n} eleman içermelidir, {values.Length} eleman girildi!");

            int[,] matrix = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i, j] = values[i * n + j];

            return matrix;
        }

        private static int[,] MatrixInverseMod29(int[,] matrix, int n)
        {
            const int mod = 29;

            int[,] aug = new int[n, 2 * n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    aug[i, j] = ((matrix[i, j] % mod) + mod) % mod;
                aug[i, n + i] = 1;
            }

            for (int col = 0; col < n; col++)
            {
                int pivotRow = -1;
                for (int row = col; row < n; row++)
                {
                    if (aug[row, col] != 0)
                    {
                        pivotRow = row;
                        break;
                    }
                }

                if (pivotRow == -1)
                    throw new InvalidOperationException(
                        "Anahtar matrisinin tersi alınamaz! (Determinant ≡ 0 mod 29)");

                if (pivotRow != col)
                {
                    for (int j = 0; j < 2 * n; j++)
                        (aug[col, j], aug[pivotRow, j]) = (aug[pivotRow, j], aug[col, j]);
                }

                int pivotInv = ModInverse(aug[col, col], mod);
                if (pivotInv == -1)
                    throw new InvalidOperationException(
                        "Anahtar matrisinin tersi alınamaz! (Pivot elemanının mod 29 tersi yok)");

                for (int j = 0; j < 2 * n; j++)
                    aug[col, j] = (aug[col, j] * pivotInv) % mod;

                for (int row = 0; row < n; row++)
                {
                    if (row != col && aug[row, col] != 0)
                    {
                        int factor = aug[row, col];
                        for (int j = 0; j < 2 * n; j++)
                            aug[row, j] = (((aug[row, j] - factor * aug[col, j]) % mod) + mod) % mod;
                    }
                }
            }

            int[,] inv = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    inv[i, j] = aug[i, n + j];

            return inv;
        }

        // ──────────────────────────────────────────────
        //  11. RSA ŞİFRE ÇÖZME
        //  Encode: C = M^e (mod n)
        //  Decode: M = C^d (mod n),  d·e ≡ 1 (mod φ(n)),  φ(n) = (p−1)(q−1)
        // ──────────────────────────────────────────────

        private static string DecodeRsa(string sifreliMetin, int asalP, int asalQ, string acikUsuMetni)
        {
            if (asalP <= 1 || asalQ <= 1)
                throw new InvalidOperationException("p ve q asal sayıları 1'den büyük olmalıdır.");

            long modulN = (long)asalP * asalQ;
            if (modulN <= 0)
                throw new InvalidOperationException("n = p·q pozitif olmalıdır.");

            if (!long.TryParse(acikUsuMetni.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out long acikUsu))
                throw new InvalidOperationException("Açık üs (e) için geçerli bir tam sayı giriniz.");

            if (acikUsu <= 0)
                throw new InvalidOperationException("Açık üs (e) pozitif olmalıdır.");

            long eulerPhi = (long)(asalP - 1) * (asalQ - 1);
            long gizliUssD = ModulerTers(acikUsu, eulerPhi);
            if (gizliUssD < 0)
                throw new InvalidOperationException("e değeri φ(n) ile aralarında asal olmalıdır.");

            BigInteger modulBig = new BigInteger(modulN);
            BigInteger gizliUssBig = new BigInteger(gizliUssD);

            string[] bloklar = sifreliMetin.Split(
                new[] { ' ', '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            if (bloklar.Length == 0)
                throw new InvalidOperationException("Şifreli metinde sayı bloğu bulunamadı.");

            var sonuc = new StringBuilder();
            foreach (string blok in bloklar)
            {
                if (!BigInteger.TryParse(blok, NumberStyles.Integer, CultureInfo.InvariantCulture, out BigInteger sifreBloju))
                    throw new InvalidOperationException($"\"{blok}\" geçerli bir şifre bloğu (tam sayı) değil.");

                if (sifreBloju.Sign < 0 || sifreBloju >= modulBig)
                    throw new InvalidOperationException($"Şifre bloğu 0 ≤ C < n aralığında olmalıdır (n = {modulN}).");

                BigInteger duzMetinDegeri = BigInteger.ModPow(sifreBloju, gizliUssBig, modulBig);

                if (duzMetinDegeri.Sign < 0 || duzMetinDegeri > char.MaxValue)
                    throw new InvalidOperationException($"Çözülen değer ({duzMetinDegeri}) tek Unicode karakter aralığına sığmıyor.");

                sonuc.Append((char)(ushort)duzMetinDegeri);
            }

            return sonuc.ToString();
        }

        // Genişletilmiş Öklid algoritması ile modüler ters: d·e ≡ 1 (mod φ); yoksa -1.
        private static long ModulerTers(long e, long phi)
        {
            if (phi <= 0)
                return -1;

            long orijinalPhi = phi;
            long x0 = 0, x1 = 1;

            if (phi == 1)
                return -1;

            while (e > 1)
            {
                if (phi == 0)
                    return -1;

                long bolum = e / phi;
                long temp = phi;

                phi = e % phi;
                e = temp;

                temp = x0;
                x0 = x1 - bolum * x0;
                x1 = temp;
            }

            if (e != 1)
                return -1;

            if (x1 < 0)
                x1 += orijinalPhi;

            return x1;
        }

        // ──────────────────────────────────────────────
        //  UI YARDIMCI METOTLARI
        // ──────────────────────────────────────────────

        private static Label MakeSectionLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private static Label MakeParamLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private static Label MakeHintLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 8f, FontStyle.Italic),
                ForeColor = Color.FromArgb(130, 140, 150),
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private static NumericUpDown MakeNumericUpDown(int x, int y, int min, int max, int value)
        {
            return new NumericUpDown
            {
                Font = new Font("Consolas", 10f),
                Location = new Point(x, y),
                Size = new Size(80, 25),
                Minimum = min,
                Maximum = max,
                Value = value
            };
        }

        // ──────────────────────────────────────────────
        //  YARDIMCI
        // ──────────────────────────────────────────────

        private static void ShowWarning(string msg) =>
            MessageBox.Show(msg, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private static void ShowError(string msg) =>
            MessageBox.Show(msg, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
