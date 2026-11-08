# CSE 325 – W01 Notes

## 1. Pizza list (existing + my additional pizza)

Request: `GET https://localhost:7151/pizza`

```json
[
  { "id": 1, "name": "Classic Italian", "isGlutenFree": false },
  { "id": 2, "name": "Veggie", "isGlutenFree": true },
  { "id": 3, "name": "Hawaiian", "isGlutenFree": false }
]
```
The Hawaiian pizza (Id 3) is my additional pizza added to the seed data in `PizzaService`.

## 2. API tests

### GET
- Request: `curl -i -k https://localhost:7151/pizza/3`
- Response: `{"id":3,"name":"Hawaiian","isGlutenFree":false}`
- Status code: **200 OK**

### POST
- Request:
  ```bash
  curl -i -k -X POST https://localhost:7151/pizza \
    -H "Content-Type: application/json" \
    -d '{"name":"Pepperoni","isGlutenFree":false}'
  ```
- Response: `{"id":4,"name":"Pepperoni","isGlutenFree":false}` with header `location: https://localhost:7151/Pizza/4`
- Status code: **201 Created**

### PUT
- Request:
  ```bash
  curl -i -k -X PUT https://localhost:7151/pizza/4 \
    -H "Content-Type: application/json" \
    -d '{"id":4,"name":"Pepperoni Deluxe","isGlutenFree":true}'
  ```
- Response: empty body (verified afterwards with `GET /pizza/4` → `{"id":4,"name":"Pepperoni Deluxe","isGlutenFree":true}`)
- Status code: **204 No Content**

### DELETE
- Request: `curl -i -k -X DELETE https://localhost:7151/pizza/4`
- Response: empty body (verified afterwards with `GET /pizza` → only pizzas 1–3 remain)
- Status code: **204 No Content**


## 3. Sales summary function

```csharp
void GenerateSalesSummaryReport(IEnumerable<string> salesFiles, string storesDirectory, string reportPath)
{
    var usCulture = CultureInfo.GetCultureInfo("en-US");
    var details = new StringBuilder();
    double total = 0;

    foreach (var file in salesFiles)
    {
        // Only include the daily sales files
        if (Path.GetFileName(file) != "sales.json")
            continue;

        string json = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(json);
        double fileTotal = data?.Total ?? 0;
        total += fileTotal;

        // Show "201/sales.json" instead of just "sales.json"
        string name = Path.GetRelativePath(storesDirectory, file);
        details.AppendLine($"  {name}: {fileTotal.ToString("C", usCulture)}");
    }

    var report = new StringBuilder();
    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");
    report.AppendLine($" Total Sales: {total.ToString("C", usCulture)}");
    report.AppendLine();
    report.AppendLine(" Details:");
    report.Append(details);

    File.WriteAllText(reportPath, report.ToString());
}
```

Output (`salesTotalDir/salesSummary.txt`):

```text
Sales Summary
----------------------------
 Total Sales: $2,012.20

 Details:
  sales.json: $88.88
  204/sales.json: $88.88
  203/sales.json: $99.00
  202/sales.json: $1,234.22
  201/sales.json: $501.22
```