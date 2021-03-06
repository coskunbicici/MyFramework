using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;

        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            //business codes : örn. kredi uygulamasında kişinin kredi almaya uygun olup olmadığının kontrolünün yapılması
            // ehliyet uygulamasında kişinin trafik, motor, ilk yardım sınavından 70 alıp ehliyet almaya hak kazanması gibi 
            // karar kontrol işlemleri kodlanır
            //validation : doğrulama (verinin yapısal olarak doğru olup olmadığı kontrolü)
            // ikisi birbirinden ayrı olmalı

            //// ----- Fluent Validation implementation (Kötü Kod) -----
            //var context = new ValidationContext<Product>(product);
            //ProductValidator productValidator = new ProductValidator();
            //var result = productValidator.Validate(context);
            //if (!result.IsValid)
            //{
            //    throw new ValidationException(result.Errors);
            //}
            //// ----- Fluent Validation implementation (Kötü Kod) -----
            //ValidationTool.Validate(new ProductValidator(), product);

            // İş kuralı: Bir kategoride en fazla 10 ürün olabilir
            // İş kuralı: Aynı isimde ürün eklenemez
            // İş kuralı: Kategori sayısı 15'i geçmişse ürün eklenemez
            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfProductNameNotExistCorrect(product.ProductName),
                CheckIfCategoryLimitExceded());
            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);

        }


        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        [TransactionScopeAspect]
        [PerformanceAspect(90)]  // bu methodun çalışması 90 saniye'yi geçerse beni uyar
        public IResult AddTransactionalTest(Product product)
        {
            Add(product);
            if (product.UnitPrice < 20)
            {
                throw new Exception("Transaction test error");
            }
            Add(product);

            return null;
        }


        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]  // içinde geçen cache'i siler
        public IResult Update(Product product)
        {
            throw new NotImplementedException();
        }

        [CacheAspect] // key, value => örn: Business.Concrete.ProductManager.GetAll
        [PerformanceAspect(120)]  // bu methodun çalışması 120 saniye'yi geçerse beni uyar
        public IDataResult<List<Product>> GetAll()
        {
            //İş kodları
            //Yetkisi var mı?
            if (DateTime.Now.Hour == 2)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        [CacheAspect]
        [PerformanceAspect(5)]  // bu methodun çalışması 5 saniye'yi geçerse beni uyar
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        // İş kuralı parçacığı farklı yerlerde kullanmayacağız
        // Zaten farklı yerlerde kullanılması gereken birşeyse onu ProductService'te yazmamız gerekir
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(f => f.CategoryId == categoryId).Count;
            if (result >= 100)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameNotExistCorrect(string productName)
        {
            var result = _productDal.GetAll(f => f.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameExistsError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceeded);
            }
            return new SuccessResult();
        }

        
    }
}
