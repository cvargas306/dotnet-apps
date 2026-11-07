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