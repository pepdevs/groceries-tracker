# Food Expense Tracker — Full Technical Spec

## 1. Document intent

This document defines the **full MVP technical specification** for a personal grocery expense tracking system that ingests receipt images, extracts structured purchase data, normalizes products, stores historical price observations, and generates useful spending and price reports.

This version is optimized in this priority order:

1. **Clean architecture / maintainability**
2. **AI-assisted development friendliness**
3. **Speed of MVP delivery**

That means the system is deliberately structured so you can build it incrementally with GitHub Copilot or another capable coding model without letting the codebase turn into mud.

---

## 2. Product goal

The system should help answer questions like:

- Where am I spending the most?
- Which store is taking the biggest share of my grocery budget?
- How much am I spending on categories like plant-based milk, bread, cleaning products, cookies, or fresh food?
- Is the latest price I paid for a given product above or below what I usually pay?
- How has the price of the same product evolved over time?
- Are there cheaper alternatives across stores for products I buy frequently?

The core problem is **not OCR alone**. The core problem is:

- ingestion of receipts with low friction
- reliable extraction of date, shop, product lines, and prices
- normalization of messy receipt text into canonical products
- preserving history in a queryable format
- providing reports that are accurate enough to influence buying decisions

---

## 3. Supported stores

Initial supported stores:

- Casa Ametller
- Bonpreu Esclat
- Massa Viva
- Espiga d’Or

Receipt input sources:

- Casa Ametller: image exports/screenshots from mobile app
- Bonpreu Esclat: image exports/screenshots from mobile app
- Massa Viva: photos/scans of physical paper receipts
- Espiga d’Or: photos/scans of physical paper receipts

---

## 4. Scope

### 4.1 In scope for MVP

- Manual upload of receipt images
- OCR of uploaded images
- Storage of original images
- Storage of raw OCR text
- Detection of store
- Extraction of purchase date
- Extraction of receipt line items
- Extraction of line prices
- Review UI to correct parsing mistakes
- Product normalization into canonical products
- Persistent historical storage
- Basic duplicate detection
- Reporting:
  - spend by store
  - spend by category
  - spend by product
  - latest vs historical average
  - product price history
  - same product across stores

### 4.2 Out of scope for MVP

- Native mobile app
- Direct supermarket API integration
- Fully automated import from apps
- Perfect discount allocation
- Barcode database enrichment
- Inventory tracking at home
- Pantry management
- Meal planning
- Multi-user households
- Bank transaction matching
- Per-gram or per-liter normalization for all products
- Real-time notifications

---

## 5. Success criteria

The MVP is successful if:

- A receipt from any of the four supported stores can be uploaded and processed into a reviewable draft
- Most receipts can be finalized with low manual effort
- Historical data remains consistent enough for reliable reporting
- The user can answer:
  - where money goes
  - which categories are expensive
  - whether a recent purchase was unusually costly
  - whether a product is getting more expensive over time
- The codebase remains easy to extend when adding new parsers, new reports, or better matching logic

---

## 6. Design principles

### 6.1 Preserve raw truth

Never discard original source data.

Every finalized purchase must remain traceable back to:

- original image
- OCR output
- parsed raw lines
- normalization decision

### 6.2 Separate pipeline stages

Keep these stages distinct:

1. File ingestion
2. OCR
3. Receipt parsing
4. Product matching
5. User review
6. Finalization
7. Reporting

This reduces debugging pain and lets you improve one stage without breaking the whole system.

### 6.3 Human-in-the-loop first

Do not assume OCR or AI will be perfect.

The system should be designed so that:

- automatic suggestions save time
- manual review guarantees data quality
- corrections feed future automation

### 6.4 Alias learning is core value

The most valuable long-term asset is not the OCR engine. It is the **product alias memory** built from repeated tickets and manual corrections.

### 6.5 Boring architecture beats clever architecture

Use familiar patterns, clear modules, and testable services. Do not build an over-engineered event-driven system for a personal MVP.

---

## 7. High-level architecture

### 7.1 Main components

- **Web UI**  
  Upload receipts, review drafts, browse reports, inspect products

- **API layer**  
  Exposes endpoints for upload, draft retrieval, confirmation, search, and reporting

- **Application services**  
  Orchestrate OCR, parsing, matching, duplicate detection, and finalization

- **Domain layer**  
  Entities, value objects, domain rules, enums

- **Infrastructure layer**  
  Database access, file storage, OCR providers, optional AI providers, migrations

- **Database**  
  Persists receipts, OCR lines, raw parsed lines, products, aliases, purchase history

- **File storage**  
  Stores original receipt images

### 7.2 Recommended stack

- Backend: ASP.NET Core Web API
- ORM: Entity Framework Core
- DB for prototype: SQLite
- DB for durable hosted version: PostgreSQL
- Frontend:
  - pragmatic option: server-rendered UI / Razor / Blazor Server
  - richer option: React frontend over API
- Image storage:
  - local disk in development
  - blob/object storage later
- OCR: pluggable provider behind interface
- Optional AI normalization helper behind interface

---

## 8. Recommended repository structure

```text
food-expense-tracker/
│
├── src/
│   ├── FoodTracker.Api/
│   ├── FoodTracker.Application/
│   ├── FoodTracker.Domain/
│   ├── FoodTracker.Infrastructure/
│
├── tests/
│   ├── FoodTracker.Api.Tests/
│   ├── FoodTracker.Application.Tests/
│   ├── FoodTracker.Domain.Tests/
│   ├── FoodTracker.Infrastructure.Tests/
│
├── docs/
│   ├── technical-spec.md
│   ├── technical-spec-full.md
│   ├── architecture-decisions.md
│   ├── copilot-prompts.md
│
├── scripts/
│
├── .gitignore
├── README.md
└── FoodExpenseTracker.sln
```

### Why this repo layout

- **Domain** stays pure and stable
- **Application** contains orchestration and use cases
- **Infrastructure** contains volatile integrations
- **API** stays thin
- **docs** becomes the source of truth for AI prompts and implementation consistency

This is the right balance for your priority order.

---

## 9. User workflow

### 9.1 Upload workflow

1. User uploads one or more receipt images
2. System stores file and computes hash
3. System creates receipt record in `Uploaded` state
4. OCR runs
5. Raw OCR result is stored
6. Parser extracts store/date/lines/prices
7. Matching service suggests canonical products
8. Draft is shown in review UI
9. User edits mistakes
10. User confirms receipt
11. Purchase items are finalized and available to reports

### 9.2 Historical analysis workflow

1. User opens dashboard
2. User sees aggregate spend data
3. User searches a product or category
4. System shows:
   - spend trends
   - purchase history
   - store comparison
   - latest price vs rolling average

---

## 10. Core domain concepts

### 10.1 Store

Represents a supported shop.

Examples:
- Casa Ametller
- Bonpreu Esclat
- Massa Viva
- Espiga d’Or

### 10.2 Receipt

Represents one uploaded ticket, whether app image or physical ticket photo.

### 10.3 OCR line

Represents line-level OCR output before parsing logic classifies it.

### 10.4 Raw parsed line

Represents the parser’s attempt to identify a receipt line as a product line or ignorable line.

### 10.5 Product

Represents a canonical product definition used for historical comparison.

### 10.6 Product alias

Represents mapping from messy receipt wording to a canonical product.

### 10.7 Purchase item

Represents the final normalized purchase line after review and confirmation.

### 10.8 Category

Represents a product grouping for reporting.

Examples:
- Bread
- Plant-based milk
- Fresh fruit
- Vegetables
- Cleaning
- Kitchen paper
- Cookies
- Tofu
- Yogurts

---

## 11. Data model

## 11.1 Store

Fields:

- `Id`
- `Code`
- `Name`
- `IsActive`
- `CreatedAt`
- `UpdatedAt`

Notes:

- `Code` should be stable and short, e.g. `CASA_AMETLLER`, `BONPREU_ESCLAT`
- Seed these stores in the DB from the beginning

---

## 11.2 Receipt

Fields:

- `Id`
- `StoreId` nullable
- `PurchaseDate` nullable
- `PurchaseTime` nullable
- `ReceiptNumber` nullable
- `TotalAmount` nullable
- `Currency`
- `OriginalFileName`
- `StoredFilePath`
- `FileHash`
- `RawOcrText`
- `OcrStatus`
- `ParsingStatus`
- `NeedsReview`
- `SourceType`
- `DuplicateCheckStatus`
- `CreatedAt`
- `UpdatedAt`

Notes:

- `SourceType` can distinguish app screenshot vs physical photo
- `NeedsReview` should usually remain true until final confirmation
- `RawOcrText` should always be stored even if parsing later fails

---

## 11.3 ReceiptOcrLine

Fields:

- `Id`
- `ReceiptId`
- `LineNumber`
- `Text`
- `Confidence` nullable
- `BoundingBoxJson` nullable
- `CreatedAt`

Purpose:

- preserve OCR line structure
- support debugging
- allow parser improvements later

---

## 11.4 ReceiptLineRaw

Fields:

- `Id`
- `ReceiptId`
- `SourceOcrLineNumber` nullable
- `RawText`
- `DetectedPrice` nullable
- `DetectedQuantity` nullable
- `DetectedUnit` nullable
- `IsProductLine`
- `IgnoredReason` nullable
- `ParseConfidence` nullable
- `CreatedAt`

Purpose:

- parser output before final normalization
- keeps a stable record of what the parser thought it saw

Ignored line examples:

- TOTAL
- SUBTOTAL
- IVA
- VISA
- TARJETA
- loyalty footers
- addresses
- phone numbers
- cashier messages

---

## 11.5 Category

Fields:

- `Id`
- `Name`
- `ParentCategoryId` nullable
- `CreatedAt`

Initial suggested categories:

- Bread
- Plant-based milk
- Vegetables
- Fruit
- Legumes
- Tofu / plant proteins
- Yogurts / desserts
- Cookies / snacks
- Cleaning
- Kitchen paper
- Pantry staples
- Frozen
- Other

Do not overcomplicate category taxonomy in MVP.

---

## 11.6 Product

Fields:

- `Id`
- `NormalizedName`
- `CategoryId`
- `Subtype` nullable
- `Brand` nullable
- `SizeValue` nullable
- `SizeUnit` nullable
- `ComparableGroup` nullable
- `Notes` nullable
- `IsActive`
- `CreatedAt`
- `UpdatedAt`

Examples:

- `Soy milk 1L`
- `Oat milk 1L`
- `Whole wheat loaf`
- `Sourdough loaf`
- `Kitchen paper pack`
- `Tofu natural 200g`

Notes:

- `ComparableGroup` helps compare substitutes later
- `Brand` is optional in MVP
- `SizeValue` and `SizeUnit` should be nullable; do not force extraction when unclear

---

## 11.7 ProductAlias

Fields:

- `Id`
- `StoreId` nullable
- `RawText`
- `NormalizedRawText`
- `ProductId`
- `MatchMethod`
- `Confidence` nullable
- `CreatedFromManualReview`
- `LastUsedAt` nullable
- `CreatedAt`

Purpose:

- maps repeated ticket wording to canonical products
- can be store-specific or global
- is the main way the system improves over time

Examples:

- `BEG SOJA 1L` → `Soy milk 1L`
- `BEBIDA AVENA` → `Oat milk 1L`
- `PA INTGRAL` → `Whole wheat loaf`

---

## 11.8 PurchaseItem

Fields:

- `Id`
- `ReceiptId`
- `ReceiptLineRawId`
- `ProductId`
- `StoreId`
- `PurchaseDate`
- `RawName`
- `Quantity` nullable
- `Unit` nullable
- `UnitPrice` nullable
- `LineTotal`
- `DiscountAmount` nullable
- `WasAutoMatched`
- `ReviewStatus`
- `CreatedAt`
- `UpdatedAt`

Purpose:

- final normalized purchase line for reporting

This is the table most reports will query.

---

## 11.9 ProcessingLog

Fields:

- `Id`
- `ReceiptId`
- `Step`
- `Status`
- `Message`
- `CreatedAt`

Purpose:

- observability
- easier debugging
- transparency when OCR or parsing fails

---

## 12. Enums and statuses

### 12.1 OCR status

Suggested values:

- `Pending`
- `Completed`
- `Failed`

### 12.2 Parsing status

Suggested values:

- `Pending`
- `Parsed`
- `Failed`
- `NeedsReview`
- `Finalized`

### 12.3 Review status for purchase items

Suggested values:

- `AutoMatched`
- `ManuallyReviewed`
- `NeedsAttention`

### 12.4 Match method

Suggested values:

- `ExactStoreAlias`
- `ExactGlobalAlias`
- `FuzzyAlias`
- `StructuredHeuristic`
- `AiSuggested`
- `Manual`

---

## 13. Product normalization strategy

This is the core of the system.

A raw receipt line is not the product identity. The system needs a canonical representation that survives OCR noise, abbreviations, and shop-specific naming.

### 13.1 Canonical product shape

A product should be modeled with these normalization layers:

- category
- subtype
- brand optional
- size optional
- normalized name

Example:

Raw inputs:
- `BEG SOJA`
- `SOJA 1L`
- `BEBIDA SOJA 1L`
- `LLET SOJA`

Canonical result:
- Category: `Plant-based milk`
- Subtype: `Soy milk`
- Brand: `unknown` or store brand
- Size: `1 L`
- NormalizedName: `Soy milk 1L`

### 13.2 Comparison levels

The system should support three practical levels of comparison:

#### Level 1: exact product
Compare the same canonical product over time.

#### Level 2: same family/category
Compare spend on broader groups like all plant-based milks.

#### Level 3: same comparable group
Compare substitutes such as:
- Oatly Oat Milk 1L
- Bonpreu Oat Drink 1L
- Casa Ametller Oat Drink 1L

This can be introduced progressively. The schema already leaves room for it.

### 13.3 Manual creation over forced perfection

If the parser or AI cannot confidently determine exact subtype or size, allow a simpler canonical product in MVP rather than blocking save.

Bad:
- forcing a wrong product

Good:
- saving as a broader but consistent product
- improving later with remapping

---

## 14. Matching algorithm

The matching algorithm should be layered and deterministic-first.

### 14.1 Preprocessing

Before matching, normalize raw line text:

- uppercase
- trim whitespace
- collapse repeated spaces
- optionally strip accents for matching
- normalize separators
- standardize known abbreviations

Examples:

- `Beg. Soja 1 L`
- `BEG SOJA 1L`
- `beg soja 1 l`

should reduce to similar internal matching text.

### 14.2 Matching order

1. **Exact store-specific alias**
2. **Exact global alias**
3. **Fuzzy alias match**
4. **Structured heuristic extraction**
5. **Optional AI suggestion**
6. **Manual review**

### 14.3 AI usage policy

AI should not be the first line of matching.

AI may be used to suggest:

- probable category
- probable product normalization
- subtype extraction
- size extraction when wording is cryptic

But AI output should be treated as a suggestion, not truth.

### 14.4 Manual review creates memory

When the user corrects a mapping, the system should:

- create or update a `ProductAlias`
- mark it as `CreatedFromManualReview = true`
- reuse it automatically later

This is how the system becomes genuinely useful over time.

---

## 15. OCR requirements

### 15.1 OCR responsibilities

OCR should do only this:

- convert image to text
- preferably preserve line layout
- optionally provide confidence and bounding boxes

### 15.2 OCR non-responsibilities

OCR should not be responsible for:

- deciding what is a product
- deciding what category a line belongs to
- deciding how to normalize the product

Those are parser and matching responsibilities.

### 15.3 OCR provider abstraction

Create an interface like:

- `IOcrProvider`

Return a normalized internal model such as:

- full text
- collection of OCR lines
- confidence data where available

This keeps the system flexible and AI-friendly.

---

## 16. Receipt parsing strategy

The parser should be modular and conservative.

### 16.1 Store detection

Store detection can use:

- known header keywords
- known branding patterns
- store-specific regex rules

Maintain a per-store configuration or parser strategy.

### 16.2 Date extraction

Date extraction should be rule-based first using regex and known formats, for example:

- `24/03/2026`
- `24-03-2026`
- `2026-03-24`

Also support optional time extraction if present.

### 16.3 Product line detection

Use line heuristics such as:

- line ends with a valid monetary value
- line appears in the body section before totals
- line does not match ignored keywords
- line is not clearly footer/header information

### 16.4 Price extraction

Prefer the last price-like token on the line.

Need to support:

- comma decimal separators
- dot decimal separators
- euro-style formatting

### 16.5 Parser per store vs generic parser

Recommended approach:

- generic base parser for common behaviors
- store-specific parser profiles or classes for exceptions

Possible implementation:

- `IReceiptParser`
- `IStoreParser`
- `GenericReceiptParser`
- `CasaAmetllerParser`
- `BonpreuEsclatParser`
- `MassaVivaParser`
- `EspigaDOrParser`

Even if the first versions are thin wrappers, this structure pays off later.

---

## 17. Duplicate detection

Receipts may be uploaded twice.

### 17.1 Hard duplicates

Use file hash comparison.

If identical file hash exists, flag as hard duplicate candidate.

### 17.2 Soft duplicates

Use a fingerprint combining:

- store
- purchase date
- purchase time if available
- total amount
- OCR similarity

Soft duplicates should trigger warning, not automatic deletion.

### 17.3 UX behavior

If a possible duplicate is found:

- show warning in review UI
- allow user to continue or cancel
- never silently discard user data

---

## 18. Review workflow

This is the most important UI after upload.

### 18.1 Review screen responsibilities

The user must be able to:

- inspect image preview
- inspect raw OCR text
- edit store
- edit purchase date
- edit line price
- mark line as ignored
- choose a canonical product
- create a new canonical product
- adjust category
- fix wrong matches
- see confidence indicators / warnings

### 18.2 Review screen layout

Recommended sections:

1. **Receipt header**
   - image thumbnail
   - store
   - date
   - total
   - duplicate warning if any

2. **Raw OCR panel**
   - collapsible
   - useful for debugging

3. **Parsed lines grid**
   - raw line text
   - detected price
   - suggested product
   - category
   - confidence
   - action buttons

4. **Quick create product dialog**
   - normalized name
   - category
   - subtype
   - brand optional
   - size optional

### 18.3 Review save behavior

Final confirmation should:

- persist updated receipt header
- persist or update raw parsed lines
- create aliases for manual corrections
- create finalized purchase items
- mark receipt as finalized

---

## 19. Reporting requirements

Reports should be based on finalized purchase items only.

### 19.1 Overview dashboard

Show:

- total spend this month
- total spend last month
- top store by spend
- top category by spend

### 19.2 Spend by store

Aggregate total spend over a date range grouped by store.

### 19.3 Spend by category

Aggregate total spend grouped by category.

### 19.4 Spend by product

Aggregate total spend grouped by canonical product.

### 19.5 Product history

For a selected product, show:

- all purchases over time
- store
- date
- line total
- unit price if available

### 19.6 Latest vs rolling average

For a selected product, compare:

- latest paid price
- average over previous 30 / 90 days
- delta percentage

### 19.7 Cross-store comparison

For a selected product, compare:

- average line total by store
- latest purchase per store
- count of purchases per store

### 19.8 Future report extensions

Leave room for:

- biggest recent price increases
- frequently bought products
- top products by month
- category inflation trends
- substitution opportunities

---

## 20. Business rules

### 20.1 Finalization rules

A receipt should not be finalized unless:

- store is known
- purchase date is known
- at least one line is mapped to a product

### 20.2 Product line rules

Every parsed candidate line must end up in one of two states:

- mapped to a canonical product
- marked as ignored/non-product

No ambiguous half-state should remain on finalized receipts.

### 20.3 Edit rules

Historical corrections should be allowed, but ideally through explicit edit flows rather than hidden DB mutations.

---

## 21. Edge cases and limitations

### 21.1 Fresh produce sold by weight

Receipts may include lines where only line total is obvious.

For MVP:

- store line total
- store quantity/unit if detectable
- do not block save when exact unit price is missing

### 21.2 Discounts

Discounts may appear:

- inline on product lines
- as separate negative lines
- as summary lines

For MVP:

- preserve detected line total
- capture explicit discount amount if obvious
- avoid complex discount allocation logic initially

### 21.3 Cryptic bread receipt wording

Bread shops may use short abbreviations. Expect higher manual alias building at the beginning.

### 21.4 OCR failures on low-quality physical tickets

When OCR confidence is poor:

- keep receipt in reviewable state
- allow manual correction
- do not lose upload

---

## 22. API design

## 22.1 Receipts

### `POST /api/receipts/upload`

Uploads a receipt image.

Response:
- receipt id
- upload status
- duplicate warning if immediate hard duplicate found

### `POST /api/receipts/{id}/process`

Runs OCR and parsing if not triggered automatically.

Response:
- processing status

### `GET /api/receipts/{id}`

Returns full receipt detail.

### `GET /api/receipts/{id}/draft`

Returns the editable review draft:
- header fields
- OCR text
- parsed lines
- candidate matches
- warnings

### `POST /api/receipts/{id}/confirm`

Finalizes receipt.

Payload includes:
- corrected store/date
- corrected lines
- mapping choices
- newly created products if any

### `GET /api/receipts`

Optional listing endpoint with filters:
- date range
- store
- status

---

## 22.2 Products

### `GET /api/products/search?q=...`

Search canonical products.

### `POST /api/products`

Create product manually.

### `GET /api/products/{id}`

Get product detail.

### `GET /api/categories`

List categories.

### `POST /api/aliases`

Create or update alias mapping.

---

## 22.3 Reports

### `GET /api/reports/spend-by-store?from=...&to=...`

### `GET /api/reports/spend-by-category?from=...&to=...`

### `GET /api/reports/top-products?from=...&to=...`

### `GET /api/reports/product-price-history/{productId}`

### `GET /api/reports/product-latest-vs-average/{productId}?windowDays=90`

### `GET /api/reports/cross-store-comparison/{productId}`

---

## 23. DTO design guidance

Keep DTOs explicit. Do not expose EF entities directly.

Suggested DTO groups:

- upload DTOs
- receipt draft DTOs
- review confirmation DTOs
- product search DTOs
- reporting DTOs

Especially important:

### Receipt draft DTO

Should include:

- receipt metadata
- OCR text
- raw parsed lines
- suggested product candidates
- duplicate warnings
- confidence indicators

### Confirm receipt DTO

Should include:

- corrected store/date fields
- each line’s final state
- mapped product id or new product definition
- ignored lines

This is a great place to keep Copilot focused.

---

## 24. Suggested code interfaces

Recommended application interfaces:

- `IReceiptStorageService`
- `IOcrProvider`
- `IReceiptParser`
- `IStoreParser`
- `IProductMatchingService`
- `IDuplicateDetectionService`
- `IReceiptDraftService`
- `IReceiptFinalizationService`
- `IReportingService`

Suggested application use cases / handlers:

- upload receipt
- process receipt
- get receipt draft
- confirm receipt
- search products
- create product
- get spend by store
- get product history

This keeps the code generation work naturally chunked for Copilot.

---

## 25. Testing strategy

### 25.1 Domain tests

Test:

- product normalization rules
- value object behavior
- finalization preconditions

### 25.2 Application tests

Test:

- matching algorithm order
- duplicate detection logic
- finalization workflow
- alias creation from manual corrections

### 25.3 Infrastructure tests

Test:

- EF mappings
- repository behavior
- OCR provider wrappers where practical

### 25.4 API tests

Test:

- upload endpoint
- confirm endpoint
- report endpoints

### 25.5 Fixture strategy

Create sample receipt fixtures for all four stores:

- good app image
- noisy physical receipt
- duplicate case
- ambiguous product lines

This is worth doing early.

---

## 26. AI-assisted development strategy

Given your priority order, AI should be used as a **disciplined implementation assistant**, not as an autonomous architect.

### 26.1 Good AI tasks

Use Copilot for:

- entity scaffolding
- DbContext and EF configurations
- DTO creation
- endpoint boilerplate
- unit tests
- parser helper functions
- string normalization utilities
- report query scaffolding

### 26.2 Bad AI tasks

Do not ask it to:

- invent the whole architecture
- design your domain model from scratch
- freely rewrite multiple layers at once
- make product normalization decisions without constraints

### 26.3 Best prompt style

Work one slice at a time.

Examples:

- "Generate the EF Core entity and configuration for Product using this exact schema."
- "Implement a receipt draft DTO and mapping from domain entities."
- "Create a matching service that tries alias exact match, then fuzzy match, then returns unresolved."
- "Add unit tests for duplicate detection based on file hash and soft fingerprint."

This is the right way to burn Copilot credits productively.

---

## 27. MVP implementation phases

### Phase 1 — Solution skeleton

- create solution and projects
- wire references
- add docs
- add base enums and interfaces

### Phase 2 — Persistence foundation

- entities
- DbContext
- migrations
- seed stores
- local SQLite setup

### Phase 3 — Upload and storage

- file upload endpoint
- file storage abstraction
- receipt creation flow
- file hash computation

### Phase 4 — OCR integration

- `IOcrProvider`
- OCR result model
- persistence of OCR text and OCR lines

### Phase 5 — Parsing

- store detection
- date extraction
- product-line detection
- price extraction
- raw parsed line storage

### Phase 6 — Matching and normalization

- product and alias tables
- matching service
- manual alias creation flow

### Phase 7 — Review/finalization

- receipt draft endpoint
- confirmation endpoint
- creation of purchase items
- duplicate warning handling

### Phase 8 — Reporting

- spend by store
- spend by category
- spend by product
- product history
- latest vs average

### Phase 9 — Refinement

- better store-specific parsers
- improved fuzzy matching
- better UX
- edit finalized receipts
- better confidence indicators

---

## 28. Definition of done for MVP

The MVP is done when:

- you can upload receipts from all four stores
- the system stores original image and OCR text
- a review draft is produced for most receipts
- you can correct draft data without manual DB edits
- aliases are remembered for future automation
- finalized purchase history powers the core reports
- the system answers:
  - where am I spending the most?
  - how much do I spend on plant-based milk?
  - what is the latest price vs typical price?
  - which store seems cheaper for a recurring product?

---

## 29. Recommended next repo docs

Besides this file, add:

### `docs/technical-spec.md`
Shorter AI-friendly version

### `docs/architecture-decisions.md`
Track decisions like:
- SQLite first vs PostgreSQL later
- React vs server-rendered UI
- chosen OCR provider
- fuzzy match algorithm choice

### `docs/copilot-prompts.md`
Curated prompts you’ll reuse when generating code

### `README.md`
How to run locally, architecture overview, roadmap

---

## 30. Blunt recommendation

Yes, this is a good project to build with your Copilot credits.

But only if you stay in charge.

This project is ideal for AI-assisted development because it contains:

- lots of repetitive structure
- clear data models
- well-bounded services
- many testable units
- natural iteration loops

It becomes a bad AI project only when you ask the model to build the whole thing in one shot.

The right move is:

- define architecture first
- define entities and flows clearly
- implement one slice at a time
- keep docs and prompts tight
- review every generated piece

That is how you turn credits into actual speed instead of cleanup work.
