using ASP_NET_03._Razor_Pages_Product_Site.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_NET_03._Razor_Pages_Product_Site.Pages;


public class ProductModel : PageModel
{
    private readonly ProductService _service;

    public ProductModel(ProductService service)
    {
        _service = service;
    }

    public async Task OnGetAsync(int id)
    {
        var product = await _service.GetProductById(id);
        ViewData["Product"] = product;
    }
}
