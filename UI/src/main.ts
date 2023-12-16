import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { registerLicense } from '@syncfusion/ej2-base';

registerLicense('Mjk4MDYzM0AzMjMxMmUzMDJlMzBRa3BhTHVTcUF4WDY3blA3eWM5YmVXbmY3cUZuMlk2RDk3UkJ5ditNRGVVPQ==;Mjk4MDYzNEAzMjMxMmUzMDJlMzBKU0FUcVJRZ3JaUzlqMlg4NXRka2w2WHYrMUtqQ1U5NUJPTy9YSk9oUGZjPQ==;Mgo+DSMBaFt+QHJqVEZrXVNbdV5dVGpAd0N3RGlcdlR1fUUmHVdTRHRcQ19iT35Uc0ZgXnhXdXY=;Mgo+DSMBPh8sVXJ1S0R+XVFPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSHxScEVkXHxdd3xURmg=;ORg4AjUWIQA/Gnt2VFhiQlBEfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn9Rd0NjXnpdc3NdRGZZ;NRAiBiAaIQQuGjN/V0d+Xk9FdlRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31SdERlWXhbc3dSTmJaVA==;Mjk4MDYzOUAzMjMxMmUzMDJlMzBPZEgwZU5aVFBTM21YYTIvYVl1ZEgzSnZtcjhabmRXVk5wdGl1NElXWEpJPQ==;Mjk4MDY0MEAzMjMxMmUzMDJlMzBtZDd5TXp4MzZoYkNjRm5oY2dsOENMODJkYVBCejFSSC9DRHBKNjRMenJBPQ==;Mgo+DSMBMAY9C3t2VFhiQlBEfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn9Rd0NjXnpdc3NdT2Bd;Mjk4MDY0MkAzMjMxMmUzMDJlMzBPKzlTTkdJWUZwRHpEdFZtV29iK1FXSDlGY1hkNmMvT1BSTDNDczVEMitJPQ==;Mjk4MDY0M0AzMjMxMmUzMDJlMzBkejcwVGJiQ093aWUreUZRUTg0UzR5TEIzZ0RyVk1iZW9GUFNqZDB2TmFjPQ==;Mjk4MDY0NEAzMjMxMmUzMDJlMzBPZEgwZU5aVFBTM21YYTIvYVl1ZEgzSnZtcjhabmRXVk5wdGl1NElXWEpJPQ==');

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
