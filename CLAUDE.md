# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

EVANS is a comprehensive Windows Forms transportation management system built with VB.NET 2.0 and SQL Server. It manages the complete cargo transport lifecycle for Peruvian logistics companies, including shipping guides (Guías de Remisión), manifests, invoicing, and fleet management.

## Build Commands

```bash
# Build the solution (Release configuration)
msbuild EVANS.sln /p:Configuration=Release

# Build Debug configuration
msbuild EVANS.sln /p:Configuration=Debug

# Deploy to target directory
xcopy /s "bin\Release\*" "C:\Program Files\EVANS\"
```

## Architecture Overview

**Technology Stack:**
- .NET Framework 2.0 with Visual Basic.NET 9.0
- Dual SQL Server database architecture (main + yearly document databases)
- Crystal Reports 10.5 for document generation
- Windows Forms MDI application with third-party Dotnetrix TabControl

**Architecture Pattern:** Traditional 3-tier architecture
- **Presentation Layer:** Windows Forms (frm*.vb files)
- **Business Logic Layer:** Entity classes (cls*.vb files) with embedded data access
- **Data Access Layer:** SQL Server with parameterized queries within business classes

## Database Architecture

The system uses a **dual-database approach**:
- **Main Database (`EVANS`):** Master data (clients, vehicles, drivers, destinations, etc.)
- **Document Databases (`YYYY`):** Year-specific transactional data (guides, manifests, invoices)

Connection management is centralized in `modMetodos.vb` with support for both Windows and SQL Server authentication.

## Key Business Entities

**Master Data Classes:**
- `clsCliente` - Customer management with multiple addresses
- `clsVehiculo` / `clsCarreta` - Vehicle and trailer registry
- `clsChofer` - Driver management with license tracking
- `clsDestino` - Destinations with virtual distance calculation
- `clsEmpresa` - Business partners and subcontractors
- `clsUsuario` - System user management

**Transactional Classes:**
- `clsGuiaRemision` - Shipping guides (primary transport documents)
- `clsManifiesto` - Transport manifests (grouped shipments)
- `clsComprobante` - Invoices and receipts (Facturas/Boletas)
- `clsRecepcion` - Cargo reception control

## Code Conventions

**Naming Conventions:**
- Hungarian notation is used throughout (prefixes: int, str, dbl, obj, lst)
- Forms: `frm` prefix (e.g., `frmGuiaRemision`)
- Classes: `cls` prefix (e.g., `clsCliente`)
- Maintenance forms: `frmMant` prefix (e.g., `frmMantCliente`)

**Database Practices:**
- All SQL queries use parameterized statements for security
- Transactions with proper rollback mechanisms for multi-table operations
- Null-safe data handling with `NullToString()` utility
- Connection objects properly disposed in Finally blocks

**Error Handling:**
- Comprehensive Try-Catch-Finally blocks
- User-friendly error messages via MessageBox
- SQL exceptions handled separately from general exceptions

## Form Architecture Patterns

**MDI Structure:**
- `frmPrincipal` - Main MDI container with navigation menu
- Child forms instantiated and managed through MDI parent

**Form Categories:**
- **Business Forms:** Core operations (shipping guides, manifests, billing)
- **Maintenance Forms (`frmMant*`):** Master data CRUD operations  
- **Printing Forms (`frmImprimir*`):** Document printing with visual designers
- **Report Forms:** Analytics and queries
- **Utility Forms:** Login, configuration, email setup

## Document Generation System

**Visual Print Designer:**
- Drag-and-drop field positioning stored in XML configurations
- Custom font management and formatting options
- Integration with Crystal Reports for professional layouts
- Print preview functionality

**Document Types:**
- Shipping guides with configurable layouts
- Legal invoices (Facturas) and receipts (Boletas) for Peru
- Transport manifests with grouped shipment details

## Development Guidelines

**When Modifying Business Classes:**
- Always use transactions for data modifications
- Include proper error handling and rollback mechanisms
- Test with realistic data volumes
- Maintain null-safe coding practices

**When Working with Forms:**
- Follow existing MDI patterns for child form management
- Use proper disposal of database connections
- Implement consistent error handling and user feedback
- Test printing functionality thoroughly

**Database Changes:**
- Document any schema modifications
- Use proper transaction isolation levels (often Serializable for critical operations)
- Test connection pooling behavior under load

## Critical Dependencies

- Crystal Reports Runtime 10.5 required for document generation
- Dotnetrix TabControl (1.0.1.4) for enhanced UI controls
- Microsoft Office Interop Excel (11.0) for export functionality
- SQL Server 2005+ with proper permissions for dual-database access

## Testing Approach

No automated testing framework is present. Testing should focus on:
- Database transaction integrity
- Document printing quality and layout
- Multi-user scenarios with proper connection management
- Data validation and business rule enforcement
- Performance with production-scale data volumes

## Common Issues

**Performance:** 
- Archive old documents to separate yearly databases
- Monitor connection pooling and disposal
- Regular database maintenance (UPDATE STATISTICS)

**Printing:**
- Configure high-quality printer resolution settings
- Test visual designer layouts across different printers
- Verify Crystal Reports runtime installation

**Database Connectivity:**
- Verify SQL Server service status
- Check connection string configuration for both databases
- Ensure proper permissions for year-specific databases