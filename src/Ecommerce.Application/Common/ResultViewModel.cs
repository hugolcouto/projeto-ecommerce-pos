using System.Net;

namespace Ecommerce.Application.Common;

public class ResultViewModel(
    string message = "",
    bool isSuccess = true,
    HttpStatusCode? errorCode = null
)
{
    public string Message { get; set; } = message;
    public bool IsSuccess { get; set; } = isSuccess;
    public HttpStatusCode? ErrorCode { get; set; } = errorCode;

    public static ResultViewModel Success(
        IEnumerable<Queries.Products.GetAllProducts.GetAllProductsItemViewModel> productsViewModel
    ) => new();

    public static ResultViewModel Error(string message, HttpStatusCode errorCode) =>
        new(message, false, errorCode);
}

public class ResultViewModel<T>(
    T? data,
    string message = "",
    bool isSuccess = true,
    HttpStatusCode? errorCode = null
) : ResultViewModel(message, isSuccess, errorCode)
{
    public T? Data { get; set; } = data;

    public static ResultViewModel<T> Success(T data) => new(data);

    public static ResultViewModel<T> Error(string message, HttpStatusCode errorCode, T? data) =>
        new(data, message, false, errorCode);
}
