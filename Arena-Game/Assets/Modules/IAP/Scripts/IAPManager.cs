using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : cSingleton<IAPManager>, IStoreListener
{
    private Action<RequestResult> m_OnPurchaseCompleted;
    private IStoreController m_StoreController;
    private IExtensionProvider m_ExtensionProvider;
    
    void Awake()
    {
        InitializePurchasing();
    }

    private async UniTask InitializePurchasing()
    {
        InitializationOptions options = new InitializationOptions().SetEnvironmentName("production");
// #if UNITY_EDITOR || DEVELOPMENT_BUILD
//             .SetEnvironmentName("test");
// #else
//             .SetEnvironmentName("production");
// #endif

        await UnityServices.InitializeAsync(options);
        ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
        operation.completed += HandleIAPCatalogLoaded;
    }

    private void HandleIAPCatalogLoaded(AsyncOperation operation)
    {
        ResourceRequest request = operation as ResourceRequest;
        
        Debug.Log($"Loaded Asset: {request.asset}");
        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
        Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");

        StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        StandardPurchasingModule.Instance().useFakeStoreAlways = true;
        
#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.AppleAppStore));
#else
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.NotSpecified));
#endif

        foreach (ProductCatalogItem item in catalog.allProducts)
        {
            builder.AddProduct(item.id, item.type);
        }
        
        UnityPurchasing.Initialize(this, builder);
    }
    
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_ExtensionProvider = extensions;
    }

    public async UniTask<RequestResult> HandlePurchase(Product product)
    {
        // m_LoadingOverlay.SetActive(true);
        MiniLoadingScreen.Instance.ShowPage(this);
        RequestResult result = RequestResult.Failed;
        bool isCompleted = false;
        m_OnPurchaseCompleted = requestResult =>
        {
            result = requestResult;
            isCompleted = true;
        };
        m_StoreController.InitiatePurchase(product);
        await UniTask.WaitUntil((() => isCompleted));
        return result;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"Error initializing IAP because of {error}."
                  +" \r\nShow a message to the player depending on the error.");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"Error initializing IAP because of {error}."
                  +" \r\nShow a message to the player depending on the error.");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}");
        m_OnPurchaseCompleted?.Invoke(RequestResult.Success);
        m_OnPurchaseCompleted = null;
        MiniLoadingScreen.Instance.HidePage(this);
        // m_LoadingOverlay.SetActive(false);
        
        //do something, like give the player their currency, unlock the item,
        // update some metrics or analytics, etc...

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because {failureReason}");
        m_OnPurchaseCompleted?.Invoke(RequestResult.Failed);
        m_OnPurchaseCompleted = null;
        MiniLoadingScreen.Instance.HidePage(this);
        // m_LoadingOverlay.SetActive(false);
    }

    public Product GetProduct(string id)
    {
        return m_StoreController.products.all.First((product => !string.IsNullOrEmpty(id) && string.Equals(product.definition.id, id)));
    }
}