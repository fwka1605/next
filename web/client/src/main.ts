import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

import { registerLocaleData } from '@angular/common';
import ja from '@angular/common/locales/ja';

registerLocaleData(ja);

if (environment.production) {
  enableProdMode();
}

import 'hammerjs';

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.log(err));

  
