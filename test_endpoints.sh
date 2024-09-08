#!/bin/bash

BASE_URL="http://localhost:7191"
SOAP_URL="http://localhost:7191/SoapService.svc"

# Testowanie endpointów REST API

echo "Testing User endpoints..."
USER_ID=$(curl -s -X POST "$BASE_URL/api/users" -H "Content-Type: application/json" -d '{"username": "testuser", "email": "testuser@example.com"}' | jq -r '.userId')
echo "Created User ID: $USER_ID"

curl -s "$BASE_URL/api/users/$USER_ID" | jq
curl -s -X PUT "$BASE_URL/api/users/$USER_ID" -H "Content-Type: application/json" -d '{"username": "updateduser", "email": "updateduser@example.com"}' | jq
curl -s -X DELETE "$BASE_URL/api/users/$USER_ID"

echo "Testing Order endpoints..."
ORDER_ID=$(curl -s -X POST "$BASE_URL/api/orders" -H "Content-Type: application/json" -d '{"orderDate": "2023-01-01", "userId": '$USER_ID', "orderItems": [{"productId": 1, "quantity": 2}]}' | jq -r '.orderId')
echo "Created Order ID: $ORDER_ID"

curl -s "$BASE_URL/api/orders/$ORDER_ID" | jq
curl -s -X PUT "$BASE_URL/api/orders/$ORDER_ID" -H "Content-Type: application/json" -d '{"orderDate": "2023-01-02", "userId": '$USER_ID', "orderItems": [{"productId": 1, "quantity": 3}]}' | jq
curl -s -X DELETE "$BASE_URL/api/orders/$ORDER_ID"

echo "Testing Product endpoints..."
PRODUCT_ID=$(curl -s -X POST "$BASE_URL/api/products" -H "Content-Type: application/json" -d '{"name": "testproduct", "price": 10.0}' | jq -r '.productId')
echo "Created Product ID: $PRODUCT_ID"

curl -s "$BASE_URL/api/products/$PRODUCT_ID" | jq
curl -s -X PUT "$BASE_URL/api/products/$PRODUCT_ID" -H "Content-Type: application/json" -d '{"name": "updatedproduct", "price": 12.0}' | jq
curl -s -X DELETE "$BASE_URL/api/products/$PRODUCT_ID"

# Testowanie SOAP

echo "Testing SOAP endpoint..."
SOAP_REQUEST='<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
  <soap:Header/>
  <soap:Body>
    <tem:GetSoapDataAsync>
      <tem:value>123</tem:value>
    </tem:GetSoapDataAsync>
  </soap:Body>
</soap:Envelope>'

curl -s -X POST -H "Content-Type: text/xml; charset=utf-8" -H "SOAPAction: http://tempuri.org/ISoapService/GetSoapDataAsync" -d "$SOAP_REQUEST" $SOAP_URL | xmllint --format -

echo "All tests completed."
