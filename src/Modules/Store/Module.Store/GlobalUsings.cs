// Framework
global using FSH.Framework.Core.Context;
global using FSH.Framework.Core.Domain;
global using FSH.Framework.Persistence;
global using FSH.Framework.Shared.Identity.Authorization;
global using FSH.Framework.Shared.Multitenancy;
global using FSH.Module.Store.Exceptions;
global using StoreEntity = FSH.Module.Store.Domain.Store;
global using POSEntity = FSH.Module.Store.Domain.PointOfSale;

// Mediator
global using Mediator;

// Microsoft
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Utilities
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
