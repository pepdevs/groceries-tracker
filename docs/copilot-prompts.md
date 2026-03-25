# Copilot Prompt Pack

Use these prompts in order. Do not skip ahead.

Each prompt assumes the repo already contains the technical spec docs.

---

## Prompt 1 — Create the core domain enums

```text
Using docs/technical-spec-full.md as the source of truth, create the domain enums for:
- OcrStatus
- ParsingStatus
- ReviewStatus
- MatchMethod

Put them under src/FoodTracker.Domain/Enums.
Do not add extra values that are not in the spec.
```

---

## Prompt 2 — Create the core domain entities

```text
Using docs/technical-spec-full.md as the source of truth, generate the domain entities:
- Store
- Receipt
- ReceiptOcrLine
- ReceiptLineRaw
- Category
- Product
- ProductAlias
- PurchaseItem
- ProcessingLog

Requirements:
- Put them under src/FoodTracker.Domain/Entities
- Use clear property types
- Make required vs nullable fields match the spec
- Include navigation properties only where clearly useful
- Do not add business logic yet
```

---

## Prompt 3 — Create the application abstractions

```text
Create interfaces under src/FoodTracker.Application/Abstractions for:
- IReceiptStorageService
- IOcrProvider
- IReceiptParser
- IStoreParser
- IProductMatchingService
- IDuplicateDetectionService
- IReceiptDraftService
- IReceiptFinalizationService
- IReportingService

Keep method signatures minimal but practical for the MVP described in docs/technical-spec-full.md.
```

---

## Prompt 4 — Create EF Core DbContext

```text
Create AppDbContext in src/FoodTracker.Infrastructure/Persistence using the entities from the domain project.
Add DbSet properties for all aggregate tables in the spec.
Do not use data annotations; prefer Fluent API configurations.
```

---

## Prompt 5 — Create Fluent API configurations

```text
Create IEntityTypeConfiguration classes for:
- Store
- Receipt
- ReceiptOcrLine
- ReceiptLineRaw
- Category
- Product
- ProductAlias
- PurchaseItem
- ProcessingLog

Put them under src/FoodTracker.Infrastructure/Persistence/Configurations.
Requirements:
- configure keys
- configure max lengths where sensible
- configure required fields
- configure useful indexes, especially for FileHash, Store.Code, ProductAlias.NormalizedRawText, and PurchaseItem.PurchaseDate
```

---

## Prompt 6 — Seed supported stores

```text
Add seed data for the four supported stores:
- Casa Ametller
- Bonpreu Esclat
- Massa Viva
- Espiga d’Or

Use stable codes.
Place the seeding in the EF Core configuration in a maintainable way.
```

---

## Prompt 7 — Create the initial migration checklist

```text
Review the current entities and DbContext and tell me if anything is missing before creating the first EF Core migration.
Only point out real issues that would affect the migration or schema.
```

---

## Prompt 8 — Add local file storage service

```text
Implement a local disk-based IReceiptStorageService in Infrastructure.
Requirements:
- save uploaded file using a generated safe filename
- preserve original file extension
- compute and return file hash
- return stored file path
- create directories if needed
Keep the implementation simple and testable.
```

---

## Prompt 9 — Add receipt upload use case

```text
Implement the first receipt upload flow for the MVP.

Requirements:
- API endpoint: POST /api/receipts/upload
- accept a single image file
- validate allowed extensions: .jpg, .jpeg, .png
- use IReceiptStorageService to save file
- create a Receipt record with Uploaded/Pending style initial states based on the spec
- return receipt id and basic metadata
Do not implement OCR yet.
```

---

## Prompt 10 — Add API test for upload

```text
Create an integration test for POST /api/receipts/upload.
Verify:
- valid image uploads return success
- receipt record is created
- invalid extension is rejected
Keep test setup maintainable.
```

---

## Prompt 11 — Create OCR provider contracts

```text
Create internal OCR result models and update IOcrProvider to return:
- full raw text
- OCR lines
- optional confidence metadata

Do not integrate a real provider yet.
Just create the contracts and placeholder implementation shape.
```

---

## Prompt 12 — Create receipt parser contracts

```text
Design the parser contracts for the receipt parsing stage.
I need:
- a generic parser interface
- optional store-specific parser support
- a result model that includes detected store, date, total, warnings, and parsed raw lines
Use docs/technical-spec-full.md as the source of truth.
```

---

## Prompt 13 — Create product matching result models

```text
Create application models for product matching results.
The matching result should be able to represent:
- resolved match
- unresolved match
- match method
- confidence
- candidate product suggestions
Keep it simple and aligned with the spec.
```

---

## Prompt 14 — Implement basic exact alias matching

```text
Implement the first version of IProductMatchingService.

Behavior:
1. exact store-specific alias
2. exact global alias
3. otherwise unresolved

Do not add fuzzy matching yet.
Add unit tests.
```

---

## Prompt 15 — Build draft retrieval endpoint

```text
Implement GET /api/receipts/{id}/draft.

It should return:
- receipt metadata
- raw OCR text
- OCR lines if available
- parsed raw lines if available
- suggested product match info placeholder
- warnings and duplicate info placeholders if not implemented yet

Use explicit DTOs.
Do not expose EF entities directly.
```

---

## Prompt 16 — Design confirm receipt request DTO

```text
Design the request DTO for POST /api/receipts/{id}/confirm.

It needs to support:
- correcting store/date
- setting final line states
- ignoring non-product lines
- mapping lines to existing products
- creating a new product inline when needed

Keep the DTO explicit and validation-friendly.
```

---

## Prompt 17 — Implement receipt finalization

```text
Implement the first version of receipt finalization.

Requirements:
- validate store and purchase date exist
- require at least one mapped product line
- create PurchaseItem rows
- create ProductAlias rows for manual corrections where applicable
- update receipt parsing/finalization status
Add tests for validation failures and successful finalization.
```

---

## Prompt 18 — Add first report endpoint

```text
Implement GET /api/reports/spend-by-store?from=...&to=...

Requirements:
- query finalized PurchaseItems
- group by store
- return total spend and purchase count
- use explicit response DTOs
Add tests.
```

---

## Prompt 19 — Add spend by category report

```text
Implement GET /api/reports/spend-by-category?from=...&to=...

Use finalized PurchaseItems joined to Product and Category.
Return totals ordered descending by spend.
Add tests.
```

---

## Prompt 20 — Add product price history report

```text
Implement GET /api/reports/product-price-history/{productId}

Return:
- purchase date
- store
- raw name
- line total
- unit price if available

Order ascending by purchase date.
Add tests.
```

---

## Prompt 21 — Add latest vs rolling average report

```text
Implement GET /api/reports/product-latest-vs-average/{productId}?windowDays=90

Behavior:
- find latest purchase for the product
- compute average over earlier purchases within the rolling window
- return latest price, average price, delta amount, and delta percentage
Handle insufficient history gracefully.
Add tests.
```

---

## Prompt 22 — Refactor review models if needed

```text
Review the draft and confirm DTOs and refactor them for clarity if needed.
Do not change behavior unless necessary.
Focus on naming, maintainability, and alignment with docs/technical-spec-full.md.
```

---

## Usage rule

After each generated slice:
1. compile
2. run tests
3. review naming and boundaries
4. commit

Do not stack five large Copilot generations before verifying the first one.
