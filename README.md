# 🧾 Food Expense Tracker

A system to ingest grocery receipts, normalize messy product data, and generate meaningful insights about spending and price evolution.

---

## 🎯 Why this project exists

Tracking grocery expenses sounds simple — until you try to answer:

* Where am I actually spending the most?
* How much do I spend on specific products (e.g. plant-based milk)?
* Is this product getting more expensive over time?
* Which store is cheaper for the same item?

Receipts are **unstructured, inconsistent, and noisy**:

* different naming per store
* abbreviations (`BEG SOJA`, `PA INTGRAL`)
* OCR errors from photos
* mixed formats (apps vs paper tickets)

This project tackles that problem as a **data normalization and analysis system**, not just a tracker.

---

## 🧠 Core idea

The value is not OCR.

The value is:

> Turning messy receipt text into structured, comparable, and historical product data.

---

## ⚙️ What it does (MVP)

### 📥 Ingestion

* Upload receipt images (app screenshots or photos)
* Store original file and metadata
* Run OCR to extract raw text

### 🔍 Parsing

* Detect store (Casa Ametller, Bonpreu, etc.)
* Extract purchase date
* Identify product lines
* Extract prices

### 🧩 Normalization

* Map raw receipt lines to canonical products
* Learn from manual corrections (alias system)
* Handle variations like:

  * `BEG SOJA 1L`
  * `BEBIDA SOJA`
  * `LLET SOJA`

→ all mapped to a consistent product definition

### 🧾 Review (human-in-the-loop)

* Inspect OCR output
* Correct parsing mistakes
* Confirm product mappings
* Improve future automation

### 📊 Reporting

* Spend by store
* Spend by category
* Spend by product
* Product price history
* Latest price vs historical average
* Cross-store comparisons

---

## 🏗️ Architecture

This project follows a **clean architecture approach**:

```
src/
  FoodTracker.Api            → HTTP layer (controllers)
  FoodTracker.Application    → use cases / orchestration
  FoodTracker.Domain         → entities + business rules
  FoodTracker.Infrastructure → EF Core, OCR, storage
```

### Key design principles

* **Raw data is never lost**
  Every purchase is traceable back to the original receipt and OCR output

* **Pipeline separation**

  * OCR
  * parsing
  * normalization
  * review
  * reporting

* **Human-in-the-loop first**
  Automation assists, but user review ensures correctness

* **Alias learning**
  The system improves over time by remembering product mappings

---

## 🧱 Data model highlights

Core entities:

* `Receipt` → uploaded ticket
* `ReceiptOcrLine` → raw OCR lines
* `ReceiptLineRaw` → parsed candidate lines
* `Product` → canonical normalized product
* `ProductAlias` → mapping from messy text → product
* `PurchaseItem` → finalized structured purchase

This separation allows:

* reprocessing with better logic later
* safe historical comparisons
* continuous improvement

---

## 🤖 AI usage (deliberate, not blind)

AI is used as a **helper**, not the core engine.

Used for:

* suggesting product normalization
* handling ambiguous cases

Not used for:

* full parsing pipeline
* deterministic logic
* critical data decisions without review

---

## 🚀 Tech stack

* **.NET (ASP.NET Core)**
* **Entity Framework Core**
* **SQLite (MVP) → PostgreSQL (future)**
* Pluggable OCR provider
* Optional AI layer for normalization

---

## 🧪 Current status

🚧 Work in progress

Initial milestones:

* [ ] Solution structure
* [ ] Core domain entities
* [ ] Receipt upload + storage
* [ ] OCR integration
* [ ] Parsing pipeline
* [ ] Product normalization
* [ ] Review workflow
* [ ] Reporting

---

## 💡 Why this is interesting

This project is a small but realistic example of:

* working with **messy real-world data**
* building **data pipelines**
* designing **normalization strategies**
* combining **rule-based logic + AI**
* keeping **human control in the loop**

It’s closer to real business problems than typical demo apps.

---

## 📈 Future ideas

* Unit price normalization (€/L, €/kg)
* Automatic anomaly detection
* Cross-store recommendation engine
* Mobile upload flow
* Integration with store apps
* Better OCR preprocessing
* Vector search for product matching

---

## 🧭 Author intent

This project is part of a broader direction:

> Helping companies apply AI to real systems and workflows — not just demos.

If this kind of problem resonates, that’s exactly the kind of work I focus on.

---
