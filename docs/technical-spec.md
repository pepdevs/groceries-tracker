# Food Expense Tracker — Technical Spec (AI-Friendly)

## Goal

Build a personal grocery receipt tracking system that:

- ingests receipt images
- extracts store, date, product lines, and prices
- normalizes messy receipt product names
- stores historical purchase data
- produces spend and price reports

Supported stores:

- Casa Ametller
- Bonpreu Esclat
- Massa Viva
- Espiga d’Or

---

## Core principle

The real value is not OCR alone. The real value is:

- product normalization
- alias memory
- historical price tracking
- reliable review flow

---

## In scope for MVP

- Upload receipt images manually
- Run OCR
- Store original image and raw OCR text
- Detect store
- Extract purchase date
- Extract candidate product lines and prices
- Review and correct parsed results
- Map receipt lines to canonical products
- Save finalized purchase history
- Show reports:
  - spend by store
  - spend by category
  - spend by product
  - product price history
  - latest price vs rolling average

---

## Out of scope for MVP

- Native mobile app
- Automatic import from supermarket apps
- Perfect discount allocation
- Inventory tracking
- Multi-user support
- Barcode integration

---

## Architecture

Projects:

- `FoodTracker.Api`
- `FoodTracker.Application`
- `FoodTracker.Domain`
- `FoodTracker.Infrastructure`

Responsibilities:

- **Domain**: entities, enums, core rules
- **Application**: use cases and orchestration
- **Infrastructure**: EF Core, OCR provider, file storage
- **Api**: endpoints

---

## Main entities

### Store
- Id
- Code
- Name
- IsActive

### Receipt
- Id
- StoreId
- PurchaseDate
- PurchaseTime
- ReceiptNumber
- TotalAmount
- Currency
- OriginalFileName
- StoredFilePath
- FileHash
- RawOcrText
- OcrStatus
- ParsingStatus
- NeedsReview
- SourceType
- DuplicateCheckStatus
- CreatedAt
- UpdatedAt

### ReceiptOcrLine
- Id
- ReceiptId
- LineNumber
- Text
- Confidence
- BoundingBoxJson
- CreatedAt

### ReceiptLineRaw
- Id
- ReceiptId
- SourceOcrLineNumber
- RawText
- DetectedPrice
- DetectedQuantity
- DetectedUnit
- IsProductLine
- IgnoredReason
- ParseConfidence
- CreatedAt

### Category
- Id
- Name
- ParentCategoryId
- CreatedAt

### Product
- Id
- NormalizedName
- CategoryId
- Subtype
- Brand
- SizeValue
- SizeUnit
- ComparableGroup
- Notes
- IsActive
- CreatedAt
- UpdatedAt

### ProductAlias
- Id
- StoreId
- RawText
- NormalizedRawText
- ProductId
- MatchMethod
- Confidence
- CreatedFromManualReview
- LastUsedAt
- CreatedAt

### PurchaseItem
- Id
- ReceiptId
- ReceiptLineRawId
- ProductId
- StoreId
- PurchaseDate
- RawName
- Quantity
- Unit
- UnitPrice
- LineTotal
- DiscountAmount
- WasAutoMatched
- ReviewStatus
- CreatedAt
- UpdatedAt

---

## Processing pipeline

1. Upload receipt image
2. Store file and compute hash
3. Run OCR
4. Save raw OCR text and OCR lines
5. Parse receipt:
   - detect store
   - extract date
   - identify product lines
   - extract prices
6. Match raw lines to canonical products:
   - exact store alias
   - exact global alias
   - fuzzy alias
   - heuristic suggestion
   - optional AI suggestion
7. Show review draft
8. User corrects mistakes
9. Finalize receipt
10. Save purchase items and aliases

---

## Matching rules

Matching order:

1. Exact store alias
2. Exact global alias
3. Fuzzy alias
4. Structured heuristic
5. Optional AI suggestion
6. Manual user selection

Manual correction should create or update `ProductAlias`.

---

## Review screen requirements

User must be able to:

- inspect uploaded receipt
- inspect raw OCR text
- edit store
- edit purchase date
- edit prices
- ignore non-product lines
- select canonical product
- create canonical product
- correct wrong matches

---

## Main services / interfaces

- `IReceiptStorageService`
- `IOcrProvider`
- `IReceiptParser`
- `IStoreParser`
- `IProductMatchingService`
- `IDuplicateDetectionService`
- `IReceiptDraftService`
- `IReceiptFinalizationService`
- `IReportingService`

---

## Suggested statuses

### OcrStatus
- Pending
- Completed
- Failed

### ParsingStatus
- Pending
- Parsed
- Failed
- NeedsReview
- Finalized

### ReviewStatus
- AutoMatched
- ManuallyReviewed
- NeedsAttention

### MatchMethod
- ExactStoreAlias
- ExactGlobalAlias
- FuzzyAlias
- StructuredHeuristic
- AiSuggested
- Manual

---

## API endpoints

### Receipts
- `POST /api/receipts/upload`
- `POST /api/receipts/{id}/process`
- `GET /api/receipts/{id}`
- `GET /api/receipts/{id}/draft`
- `POST /api/receipts/{id}/confirm`
- `GET /api/receipts`

### Products
- `GET /api/products/search?q=...`
- `POST /api/products`
- `GET /api/products/{id}`
- `GET /api/categories`
- `POST /api/aliases`

### Reports
- `GET /api/reports/spend-by-store`
- `GET /api/reports/spend-by-category`
- `GET /api/reports/top-products`
- `GET /api/reports/product-price-history/{productId}`
- `GET /api/reports/product-latest-vs-average/{productId}?windowDays=90`
- `GET /api/reports/cross-store-comparison/{productId}`

---

## Definition of done for MVP

The MVP is done when:

- receipts from all four stores can be uploaded
- OCR output is stored
- a reviewable draft is produced
- the user can correct mistakes without touching the DB
- aliases are remembered
- historical purchase data powers reports that answer:
  - where am I spending the most?
  - how much do I spend on plant-based milk?
  - what is the latest price vs usual price?
  - which store seems cheaper for recurring products?
