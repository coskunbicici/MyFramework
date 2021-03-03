using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string ProductsListed = "Ürünler listelendi";
        public static string MaintenanceTime = "Sistem bakımı yapılmaktadır, daha sonra tekrar deneyiniz";
        public static string ProductCountOfCategoryError = "Bu kategorideki ürün sayısı limitine ulaşıldı";
        public static string ProductNameExistsError = "Aynı isimde ürün eklenemez";
        public static string CategoryLimitExceeded = "Kategori limiti aşıldığı için yeni ürün eklenemez";
        public static string AuthorizationDenied = "Yetkiniz yok";
        public static string UserRegistered = "Kullanıcı başarıyla kayıt edildi";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Giriş başarısız, kullanıcı adı veya şifrenizi kontrol ediniz";
        public static string SuccessfulLogin = "Giriş başarılı";
        public static string UserAlreadyExists = "Kullanıcı zaten var";
        public static string AccessTokenCreated = "Access Token oluşturuldu";
    }
}
