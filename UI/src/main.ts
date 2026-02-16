import { enableProdMode, provideZoneChangeDetection } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { registerLicense } from '@syncfusion/ej2-base';

registerLicense('Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXtfc3VVQmFdVkJ/WEI=');

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule, { applicationProviders: [provideZoneChangeDetection()], })
  .catch(err => console.error(err));
