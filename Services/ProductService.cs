namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IExtendedImageService _imageService;

        public ProductService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ProductService> logger,
            IExtendedImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _imageService = imageService;
        }

        #region Get Operations

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting product by ID: {ProductId}", id);

            var product = await _unitOfWork.ProductRepository.GetProductWithDetailsAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                throw new ProductNotFoundException(id);
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task<IReadOnlyList<ProductDto>> GetAllProductsAsync(bool trackChanges = false)
        {
            _logger.LogInformation("Getting all products");
            var products = await _unitOfWork.ProductRepository.GetAllAsync(trackChanges);
            var productDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);
            return productDtos;
        }

        public async Task<IReadOnlyList<ProductDto>> GetProductsByTypeAsync(ProductType productType)
        {
            _logger.LogInformation("Getting products by type: {ProductType}", productType);
            var products = await _unitOfWork.ProductRepository.GetProductsByTypeAsync(productType);
            var productDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);
            return productDtos;
        }

        public async Task<IReadOnlyList<ProductDto>> GetProductsByConditionAsync(DeviceCondition condition)
        {
            _logger.LogInformation("Getting products by condition: {Condition}", condition);
            var products = await _unitOfWork.ProductRepository.GetProductsByConditionAsync(condition);
            var productDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);
            return productDtos;
        }

        public async Task<IReadOnlyList<ProductDto>> GetActiveProductsAsync()
        {
            _logger.LogInformation("Getting active products");

            var products = await _unitOfWork.ProductRepository.GetActiveProductsAsync();
            var productDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            return productDtos;
        }

        #endregion

        #region Create/Update/Delete Operations

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
        {
            _logger.LogInformation("Creating new product: {NameEn}", createDto.NameEn);
            var existingProduct = await _unitOfWork.ProductRepository.GetByNamesAsync(createDto.NameAr, createDto.NameEn);
            if (existingProduct != null)
            {
                _logger.LogWarning("Product already exists: {NameAr}/{NameEn}", createDto.NameAr, createDto.NameEn);
                throw new ProductAlreadyExistsException(createDto.NameAr, createDto.NameEn);
            }

            Product product;
            if (createDto.ProductType == ProductType.Accessory && createDto.AccessoryType.HasValue)
            {
                product = _mapper.Map<Accessory>(createDto);
                ((Accessory)product).AccessoryType = createDto.AccessoryType.Value;
            }
            else
            {
                product = _mapper.Map<Product>(createDto);
            }

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            if (createDto.Specifications.Any())
            {
                foreach (var specDto in createDto.Specifications)
                {
                    var specification = _mapper.Map<ProductSpecification>(specDto);
                    specification.ProductId = product.Id;
                    await _unitOfWork.GetRepository<ProductSpecification, Guid>().AddAsync(specification);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            if (createDto.MediaFiles != null && createDto.MediaFiles.Any())
            {
                var mediaList = new List<ProductMedia>();
                int order = 0;

                foreach (var file in createDto.MediaFiles)
                {
                    if (file == null || file.Length == 0)
                        continue;

                    var extension = Path.GetExtension(file.FileName).ToLower();
                    string fileUrl;
                    MediaType mediaType;

                    if (_imageService.GetAllowedImageExtensions().Contains(extension))
                    {
                        fileUrl = await _imageService.SaveImageAsync(file, "uploads/products/images");
                        mediaType = MediaType.Image;
                    }
                    else if (_imageService.GetAllowedVideoExtensions().Contains(extension))
                    {
                        fileUrl = await _imageService.SaveVideoAsync(file, "uploads/products/videos");
                        mediaType = MediaType.Video;
                    }
                    else
                    {
                        _logger.LogWarning("Unsupported file type: {Extension} for product {ProductId}", extension, product.Id);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(fileUrl))
                    {
                        var isMain = createDto.Media != null &&
                                     createDto.Media.Any(m => m.Url == fileUrl && m.IsMain);
                        mediaList.Add(new ProductMedia
                        {
                            ProductId = product.Id,
                            Url = fileUrl,
                            MediaType = mediaType,
                            IsMain = isMain || (order == 0 && !mediaList.Any(m => m.IsMain)),
                            Order = order,
                            CreatedAt = DateTime.UtcNow
                        });
                        order++;
                    }
                }
                if (createDto.Media != null)
                {
                    foreach (var mediaDto in createDto.Media)
                    {
                        if (!string.IsNullOrEmpty(mediaDto.Url) && !mediaDto.Url.StartsWith("blob:", StringComparison.OrdinalIgnoreCase))
                        {
                            mediaList.Add(new ProductMedia
                            {
                                ProductId = product.Id,
                                Url = mediaDto.Url,
                                MediaType = mediaDto.MediaType,
                                IsMain = mediaDto.IsMain,
                                Order = mediaDto.Order,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }
                }

                if (mediaList.Any() && !mediaList.Any(m => m.IsMain))
                {
                    mediaList.First().IsMain = true;
                }

                foreach (var media in mediaList)
                {
                    await _unitOfWork.GetRepository<ProductMedia, Guid>().AddAsync(media);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            else if (createDto.Media != null && createDto.Media.Any())
            {
                foreach (var mediaDto in createDto.Media)
                {
                    if (!string.IsNullOrEmpty(mediaDto.Url) && !mediaDto.Url.StartsWith("blob:", StringComparison.OrdinalIgnoreCase))
                    {
                        var media = _mapper.Map<ProductMedia>(mediaDto);
                        media.ProductId = product.Id;
                        await _unitOfWork.GetRepository<ProductMedia, Guid>().AddAsync(media);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);
            return await GetProductByIdAsync(product.Id);
        }
        public async Task<ProductDto> UpdateProductAsync(UpdateProductDto updateDto)
        {
            _logger.LogInformation("Updating product: {ProductId}", updateDto.Id);

            var existingProduct = await _unitOfWork.ProductRepository.GetProductWithDetailsAsync(updateDto.Id);
            if (existingProduct == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for update", updateDto.Id);
                throw new ProductNotFoundException(updateDto.Id);
            }
            _mapper.Map(updateDto, existingProduct);
            existingProduct.UpdatedAt = DateTime.UtcNow;

            if (existingProduct is Accessory accessory && updateDto.AccessoryType.HasValue)
            {
                accessory.AccessoryType = updateDto.AccessoryType.Value;
            }

            _unitOfWork.ProductRepository.Update(existingProduct);
            await _unitOfWork.SaveChangesAsync();
            if (updateDto.Specifications?.Any() == true)
            {
                var allSpecs = await _unitOfWork.GetRepository<ProductSpecification, Guid>()
                    .GetQuery()
                    .Where(s => s.ProductId == existingProduct.Id)
                    .ToListAsync();

                if (allSpecs.Any())
                {
                    foreach (var spec in allSpecs)
                    {
                        _unitOfWork.GetRepository<ProductSpecification, Guid>().Delete(spec);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                foreach (var specDto in updateDto.Specifications!)
                {
                    var specification = _mapper.Map<ProductSpecification>(specDto);
                    specification.ProductId = existingProduct.Id;
                    specification.CreatedAt = DateTime.UtcNow;
                    await _unitOfWork.GetRepository<ProductSpecification, Guid>().AddAsync(specification);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            if (updateDto.Media?.Any() == true || (updateDto.MediaFiles != null && updateDto.MediaFiles.Any()))
            {
                var existingMedia = await _unitOfWork.GetRepository<ProductMedia, Guid>()
                    .GetQuery()
                    .Where(m => m.ProductId == existingProduct.Id)
                    .ToListAsync();

                foreach (var mediaItem in existingMedia)
                {
                    if (!string.IsNullOrEmpty(mediaItem.Url))
                    {
                        try
                        {
                            if (mediaItem.Url.Contains("/images/"))
                                await _imageService.DeleteImageAsync(mediaItem.Url);
                            else if (mediaItem.Url.Contains("/videos/"))
                                await _imageService.DeleteVideoAsync(mediaItem.Url);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete media file: {Url}", mediaItem.Url);
                        }
                    }
                    _unitOfWork.GetRepository<ProductMedia, Guid>().Delete(mediaItem);
                }
                await _unitOfWork.SaveChangesAsync();
                var tempMediaList = new List<(string Url, MediaType MediaType, int? Order, bool? IsMain, bool IsNewFile)>();
                int newFileOrderCounter = 0;
                if (updateDto.Media != null && updateDto.Media.Any())
                {
                    foreach (var mediaDto in updateDto.Media)
                    {
                        if (!string.IsNullOrEmpty(mediaDto.Url) && !mediaDto.Url.StartsWith("blob:", StringComparison.OrdinalIgnoreCase))
                        {
                            tempMediaList.Add((
                                Url: mediaDto.Url,
                                MediaType: mediaDto.MediaType,
                                Order: mediaDto.Order,
                                IsMain: mediaDto.IsMain,
                                IsNewFile: false
                            ));
                        }
                    }
                }
                if (updateDto.MediaFiles != null && updateDto.MediaFiles.Any())
                {
                    foreach (var file in updateDto.MediaFiles)
                    {
                        if (file == null || file.Length == 0) continue;

                        var extension = Path.GetExtension(file.FileName).ToLower();
                        string fileUrl;
                        MediaType mediaType;

                        if (_imageService.GetAllowedImageExtensions().Contains(extension))
                        {
                            fileUrl = await _imageService.SaveImageAsync(file, "uploads/products/images");
                            mediaType = MediaType.Image;
                        }
                        else if (_imageService.GetAllowedVideoExtensions().Contains(extension))
                        {
                            fileUrl = await _imageService.SaveVideoAsync(file, "uploads/products/videos");
                            mediaType = MediaType.Video;
                        }
                        else
                        {
                            _logger.LogWarning("Unsupported file type: {Extension}", extension);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(fileUrl))
                        {
                            tempMediaList.Add((
                                Url: fileUrl,
                                MediaType: mediaType,
                                Order: newFileOrderCounter++, 
                                IsMain: null, 
                                IsNewFile: true
                            ));
                        }
                    }
                }
                var sortedMediaList = tempMediaList
                    .OrderBy(m => m.Order ?? int.MaxValue)
                    .ToList();
                var finalMediaList = new List<ProductMedia>();
                bool hasExplicitMain = sortedMediaList.Any(m => m.IsMain == true);

                for (int i = 0; i < sortedMediaList.Count; i++)
                {
                    var mediaItem = sortedMediaList[i];
                    bool isMain;

                    if (mediaItem.IsMain.HasValue)
                    {
                        isMain = mediaItem.IsMain.Value;
                    }
                    else
                    {
                        isMain = !hasExplicitMain && i == 0;
                    }

                    finalMediaList.Add(new ProductMedia
                    {
                        ProductId = existingProduct.Id,
                        Url = mediaItem.Url,
                        MediaType = mediaItem.MediaType,
                        IsMain = isMain,
                        Order = i, 
                        CreatedAt = DateTime.UtcNow
                    });
                }
                if (finalMediaList.Any() && !finalMediaList.Any(m => m.IsMain))
                {
                    finalMediaList.First().IsMain = true;
                }
                foreach (var media in finalMediaList)
                {
                    await _unitOfWork.GetRepository<ProductMedia, Guid>().AddAsync(media);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Product updated successfully: {ProductId}", existingProduct.Id);
            return await GetProductByIdAsync(existingProduct.Id);
        }
        public async Task<bool> DeleteProductAsync(Guid id)
        {
            _logger.LogInformation("Hard deleting product: {ProductId}", id);

            var product = await _unitOfWork.ProductRepository.GetProductWithDetailsAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for deletion", id);
                throw new ProductNotFoundException(id);
            }
            if (product.Media != null)
            {
                foreach (var media in product.Media.Where(m => !m.IsDeleted))
                {
                    if (!string.IsNullOrEmpty(media.Url))
                    {
                        if (media.Url.Contains("/images/"))
                        {
                            await _imageService.DeleteImageAsync(media.Url);
                        }
                        else if (media.Url.Contains("/videos/"))
                        {
                            await _imageService.DeleteVideoAsync(media.Url);
                        }
                        else
                        {
                            await _imageService.DeleteImageAsync(media.Url);
                        }
                    }
                }
            }

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Product deleted successfully: {ProductId}", id);
            return true;
        }

        public async Task<bool> SoftDeleteProductAsync(Guid id)
        {
            _logger.LogInformation("Soft deleting product: {ProductId}", id);

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for soft deletion", id);
                throw new ProductNotFoundException(id);
            }

            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow;
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Product soft deleted successfully: {ProductId}", id);
            return true;
        }

        #endregion

        #region Price Calculation

        public async Task<PriceCalculationResultDto> CalculatePriceAsync(CalculatePriceDto calculateDto)
        {
            _logger.LogInformation("Calculating price for product: {ProductId}", calculateDto.ProductId);

            var product = await _unitOfWork.ProductRepository.GetProductWithDetailsAsync(calculateDto.ProductId);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for price calculation", calculateDto.ProductId);
                throw new ProductNotFoundException(calculateDto.ProductId);
            }

            var result = new PriceCalculationResultDto
            {
                ProductId = product.Id,
                ProductNameAr = product.NameAr,
                ProductNameEn = product.NameEn,
                BasePrice = product.BasePrice,
                FinalPrice = product.BasePrice,
                UpgradesTotal = 0,
                UpgradeBreakdown = new List<UpgradeBreakdownDto>()
            };

            foreach (var selectedUpgrade in calculateDto.SelectedUpgrades)
            {
                var productUpgrade = product.ProductUpgrades
                    .FirstOrDefault(pu => pu.UpgradeType == selectedUpgrade.UpgradeType &&
                                          pu.ToValue == selectedUpgrade.ToValue);

                if (productUpgrade != null && productUpgrade.IsActive)
                {
                    result.UpgradesTotal += productUpgrade.AdditionalPrice;
                    result.FinalPrice += productUpgrade.AdditionalPrice;

                    result.UpgradeBreakdown.Add(new UpgradeBreakdownDto
                    {
                        UpgradeType = productUpgrade.UpgradeType,
                        NameAr = productUpgrade.UpgradeOption?.NameAr ?? GetUpgradeTypeNameAr(productUpgrade.UpgradeType),
                        NameEn = productUpgrade.UpgradeOption?.NameEn ?? GetUpgradeTypeNameEn(productUpgrade.UpgradeType),
                        FromValue = productUpgrade.FromValue,
                        ToValue = productUpgrade.ToValue,
                        AdditionalPrice = productUpgrade.AdditionalPrice
                    });
                }
                else
                {
                    bool isAllowed = selectedUpgrade.UpgradeType switch
                    {
                        UpgradeType.RAM => product.AllowRamUpgrade,
                        UpgradeType.Storage => product.AllowStorageUpgrade,
                        UpgradeType.GPU => product.AllowGpuUpgrade,
                        _ => false
                    };

                    if (!isAllowed)
                    {
                        _logger.LogWarning("Upgrade type {UpgradeType} not allowed for product {ProductId}",
                            selectedUpgrade.UpgradeType, product.Id);
                        throw new UpgradeNotAllowedException(selectedUpgrade.UpgradeType.ToString(), product.Id);
                    }
                }
            }

            _logger.LogInformation("Price calculated for product {ProductId}: {FinalPrice}",
                product.Id, result.FinalPrice);

            return result;
        }

        #endregion

        #region Filter & Search

        public async Task<PagedResultDto<ProductDto>> GetFilteredProductsAsync(ProductFilterDto filterDto)
        {
            _logger.LogInformation("Getting filtered products with page {PageNumber}, size {PageSize}",
                filterDto.PageNumber, filterDto.PageSize);

            var query = _unitOfWork.ProductRepository.GetQuery()
                .Where(p => !p.IsDeleted);

            // Apply filters
            if (!string.IsNullOrWhiteSpace(filterDto.SearchTerm))
            {
                var searchTerm = filterDto.SearchTerm.ToLower();
                query = query.Where(p =>
                    p.NameEn.ToLower().Contains(searchTerm) ||
                    p.NameAr.ToLower().Contains(searchTerm) ||
                    p.DescriptionEn.ToLower().Contains(searchTerm) ||
                    p.DescriptionAr.ToLower().Contains(searchTerm));
            }

            if (filterDto.ProductType.HasValue)
                query = query.Where(p => p.ProductType == filterDto.ProductType.Value);

            if (filterDto.Condition.HasValue)
                query = query.Where(p => p.Condition == filterDto.Condition.Value);

            if (filterDto.AccessoryType.HasValue && filterDto.ProductType == ProductType.Accessory)
            {
                query = query.OfType<Accessory>()
                    .Where(a => a.AccessoryType == filterDto.AccessoryType.Value)
                    .Cast<Product>();
            }

            if (filterDto.MinPrice.HasValue)
                query = query.Where(p => p.BasePrice >= filterDto.MinPrice.Value);

            if (filterDto.MaxPrice.HasValue)
                query = query.Where(p => p.BasePrice <= filterDto.MaxPrice.Value);

            if (filterDto.IsActive.HasValue)
                query = query.Where(p => p.IsActive == filterDto.IsActive.Value);

            if (filterDto.AllowRamUpgrade.HasValue)
                query = query.Where(p => p.AllowRamUpgrade == filterDto.AllowRamUpgrade.Value);

            if (filterDto.AllowStorageUpgrade.HasValue)
                query = query.Where(p => p.AllowStorageUpgrade == filterDto.AllowStorageUpgrade.Value);

            if (filterDto.AllowGpuUpgrade.HasValue)
                query = query.Where(p => p.AllowGpuUpgrade == filterDto.AllowGpuUpgrade.Value);

            query = filterDto.SortBy?.ToLower() switch
            {
                "price" => filterDto.SortDescending
                    ? query.OrderByDescending(p => p.BasePrice)
                    : query.OrderBy(p => p.BasePrice),
                "name" => filterDto.SortDescending
                    ? query.OrderByDescending(p => p.NameEn)
                    : query.OrderBy(p => p.NameEn),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var totalCount = await Task.Run(() => query.Count());
            var products = await Task.Run(() => query
                .Skip((filterDto.PageNumber - 1) * filterDto.PageSize)
                .Take(filterDto.PageSize)
                .ToList());

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            var result = new PagedResultDto<ProductDto>
            {
                Items = productDtos,
                TotalCount = totalCount,
                PageNumber = filterDto.PageNumber,
                PageSize = filterDto.PageSize
            };

            _logger.LogInformation("Found {TotalCount} products matching filters", totalCount);
            return result;
        }

        #endregion

        #region Product Requests
        public async Task CreateProductRequestAsync(CreateProductRequestDto dto)
        {
            var request = _mapper.Map<Domain.Entities.Orders.ProductRequest>(dto);
            request.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.GetRepository<Domain.Entities.Orders.ProductRequest, Guid>().AddAsync(request);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProductRequestDto>> GetProductRequestsAsync()
        {
            var requests = await _unitOfWork.ProductRepository.GetProductRequestsAsync();
            return _mapper.Map<IEnumerable<ProductRequestDto>>(requests);
        }
        #endregion

        #region Checkers

        public async Task<bool> IsProductExistsAsync(Guid id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            return product != null;
        }

        public async Task<bool> IsProductNameExistsAsync(string nameAr, string nameEn)
        {
            var product = await _unitOfWork.ProductRepository.GetByNamesAsync(nameAr, nameEn);
            return product != null;
        }

        #endregion

        #region Helpers

        private string GetUpgradeTypeNameAr(UpgradeType upgradeType)
        {
            return upgradeType switch
            {
                UpgradeType.RAM => "رامات",
                UpgradeType.Storage => "تخزين",
                UpgradeType.GPU => "كارت شاشة",
                _ => "ترقية"
            };
        }

        private string GetUpgradeTypeNameEn(UpgradeType upgradeType)
        {
            return upgradeType switch
            {
                UpgradeType.RAM => "RAM",
                UpgradeType.Storage => "Storage",
                UpgradeType.GPU => "GPU",
                _ => "Upgrade"
            };
        }

        #endregion
    }
}