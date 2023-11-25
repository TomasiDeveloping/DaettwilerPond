import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { registerLicense } from '@syncfusion/ej2-base';

registerLicense('Mgo+DSMBaFt+QHJqVk1hXk5Hd0BLVGpAblJ3T2ZQdVt5ZDU7a15RRnVfR19gSXxTc0BjWn9deQ==;Mgo+DSMBPh8sVXJ1S0R+X1pFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jTHxQdkdjXHpec3ZVTw==;ORg4AjUWIQA/Gnt2VFhiQlJPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXtRdUVhWXpbcHdWRGk=;MjE1MDQ0OEAzMjMxMmUzMjJlMzNKM2g1N0l0Y0R2TkZxaWVSLzRNQzRVSllhbStkbkphV25LNSs0UGwxVGFRPQ==;MjE1MDQ0OUAzMjMxMmUzMjJlMzNpSkVNRlFYSmdrS25HRjA5ODh0eEliMEdyUFNodG9IcFBqbkI0ZmthNlZNPQ==;NRAiBiAaIQQuGjN/V0d+Xk9HfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5WdEZjW39bdXRRR2dV;MjE1MDQ1MUAzMjMxMmUzMjJlMzNOdFVrb3d3YlV5TnhVMGlzOGRRZXZ4T3k5QWNFT0VmUDV5SUtqZXpSTXBjPQ==;MjE1MDQ1MkAzMjMxMmUzMjJlMzNhR0JUWS8xMXBqb1hQcndxdFBDQmdzZHlQL21VaFczaHY5UzVQVTdlM0pRPQ==;Mgo+DSMBMAY9C3t2VFhiQlJPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXtRdUVhWXpbcHBdT2k=;MjE1MDQ1NEAzMjMxMmUzMjJlMzNvdmpPVy9CMk9oclFkUzczdUkzaXFMczgvYU5zbjJTSmlsZnY2YlZqU0JBPQ==;MjE1MDQ1NUAzMjMxMmUzMjJlMzNsMU5YY2E0cTNtalB4aURlVVEzWW9RTHNkeXRzWk9zOFA1ZHhQcEdiV0NvPQ==;MjE1MDQ1NkAzMjMxMmUzMjJlMzNOdFVrb3d3YlV5TnhVMGlzOGRRZXZ4T3k5QWNFT0VmUDV5SUtqZXpSTXBjPQ==');

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
