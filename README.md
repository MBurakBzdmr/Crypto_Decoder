# 🔐 Kriptoloji — Şifre Çözücü

Türk alfabesi (29 harf) üzerinde çalışan, 11 farklı kriptografik yöntemi destekleyen masaüstü şifre çözme uygulaması.

## 📋 Desteklenen Şifreleme Yöntemleri

| # | Yöntem | Açıklama |
|---|--------|----------|
| 1 | **Kaydırmalı (Caesar)** | Her harf anahtar değeri kadar geri kaydırılarak çözülür |
| 2 | **Doğrusal (Affine)** | `D(x) = a⁻¹ · (x − b) mod 29` formülüyle çözülür |
| 3 | **Yer Değiştirme (Substitution)** | 29 harflik karma alfabe ile ters eşleşme yapılır |
| 4 | **Sayı Anahtarlı (Columnar Transposition)** | Sütun transpozisyonu ile çözülür |
| 5 | **Permütasyon** | Ters permütasyon uygulanarak orijinal sıra elde edilir |
| 6 | **Rota (Spiral Transposition)** | Spiral sırayla matrise yerleştirilip satır satır okunur |
| 7 | **Zigzag (Rail Fence)** | Zigzag düzeninde raylara dağıtılıp sırayla okunur |
| 8 | **Dört Kare (Four Square)** | Dört 6×5 matris kullanılarak digram çözümü yapılır |
| 9 | **Vigenere** | Anahtar kelime ile tekrarlayan kaydırma uygulanır |
| 10 | **Hill** | Anahtar matrisinin mod 29 tersi ile matris çarpımı yapılır |
| 11 | **RSA** | `M = C^d mod n` ile özel anahtar kullanılarak çözülür |

## 🛠️ Gereksinimler

- **.NET 8.0** veya üzeri
- **Windows** (Windows Forms uygulaması)

## 🚀 Kurulum ve Çalıştırma

```bash
# Projeyi klonlayın
git clone https://github.com/KULLANICI_ADINIZ/kriptoloji-decoding.git
cd kriptoloji-decoding

# Projeyi derleyip çalıştırın
dotnet run --project kriptoloji-decoding
```

## 📸 Kullanım

1. Sol panelden şifreleme yöntemini seçin
2. Şifreli metni üst alana girin
3. Anahtar parametrelerini belirleyin
4. **ÇÖZ** butonuna tıklayın
5. Sonuç alt alanda görüntülenir

## 📄 Lisans

Bu proje açık kaynak olarak paylaşılmaktadır.
