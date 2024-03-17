import { Pipe, PipeTransform } from '@angular/core';
import {environment} from "../../environments/environment";

@Pipe({
  name: 'imageUrl',
  standalone: true
})
export class ImageUrlPipe implements PipeTransform {

  transform(imageUrl?: string): string {
    if (imageUrl) {
      return environment.serverUrl + imageUrl;
    }
    return './assets/images/no-image.png';
  }
}
