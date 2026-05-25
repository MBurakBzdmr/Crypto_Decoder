# 🔐 Kriptoloji — Şifre Çözücü (Decoder)

**Selçuk Üniversitesi Kriptoloji** dersinde öğretilen şifreleme algoritmalarının dekript (şifre çözme) işlemlerini pratik bir şekilde gerçekleştirmek amacıyla **C# (Windows Forms)** ile geliştirilmiş masaüstü uygulamasıdır. 

Uygulama, Türk alfabesi (29 harf) üzerinde çalışacak şekilde tasarlanmış olup 11 farklı popüler kriptografik algoritmanın şifre çözme işlemini desteklemektedir. Kriptoloji dersi alan öğrenciler ve algoritmaların pratik uygulamalarını test etmek isteyenler için yardımcı bir araç olarak kullanılabilir.

## 📋 Desteklenen Şifreleme Yöntemleri

| # | Yöntem | Açıklama |
|---|--------|----------|
| 1 | **Kaydırmalı (Caesar)** | Her harf anahtar değeri kadar geri kaydırılarak çözülür (`(x - k) mod 29`) |
| 2 | **Doğrusal (Affine)** | `D(x) = a⁻¹ · (x − b) mod 29` formülüyle çözülür |
| 3 | **Yer Değiştirme (Substitution)** | 29 harflik karma alfabe ile ters matris/eşleşme yapılır |
| 4 | **Sayı Anahtarlı (Columnar Transposition)** | Sütun transpozisyonu anahtarına göre bloklar düzenlenerek çözülür |
| 5 | **Permütasyon** | Ters permütasyon uygulanarak orijinal sıra elde edilir |
| 6 | **Rota (Spiral Transposition)** | Spiral/Rota sırasıyla matrise yerleştirilip okunarak çözülür |
| 7 | **Zigzag (Rail Fence)** | Zigzag (raylar) düzenindeki harflerin dizilimi çözülüp sırayla okunur |
| 8 | **Dört Kare (Four Square)** | Dört 6×5 matris kullanılarak digram (ikili harf) çözümü yapılır |
| 9 | **Vigenere** | Anahtar kelimenin her harfi kullanılarak tekrarlayan ters kaydırma uygulanır |
| 10 | **Hill** | Harflerin sayısal değerleri ile anahtar matrisinin (mod 29) tersi (inverse) çarpılır |
| 11 | **RSA** | `M = C^d mod n` formülüyle, Private Key (d) kullanılarak sayısal metin çözülür |

## 🛠️ Temel Bilgiler ve Gereksinimler

- **Alfabe:** Türk Alfabesi (A'dan Z'ye, A=0 veya A=1 düzeninde, 29 harf üzerinden mod işlemleri yapılır)
- **Altyapı:** C# / .NET 8.0 (Windows Forms)
- **İşletim Sistemi:** Windows
- **Geliştirme Ortamı:** Visual Studio 2022

## 🚀 Kurulum ve Çalıştırma

```bash
# Projeyi klonlayın
git clone https://github.com/MBurakBzdmr/Crypto_Decoder.git
cd kriptoloji-decoding

# Projeyi derleyip çalıştırın
dotnet run --project kriptoloji-decoding
```
