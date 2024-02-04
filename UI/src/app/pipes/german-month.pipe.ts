import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  // Pipe name for usage in templates
  name: 'germanMonth',

  // Optional property to indicate that this pipe is standalone (doesn't rely on Angular's change detection)
  standalone: true
})
export class GermanMonthPipe implements PipeTransform {

  // Transform function to convert numerical month to German month name
  transform(month: number): string {
    switch (month) {
      case 0: return 'Januar';
      case 1: return 'Februar';
      case 2: return 'März';
      case 3: return 'April';
      case 4: return 'Mai';
      case 5: return 'Juni';
      case 6: return 'Juli';
      case 7: return 'August';
      case 8: return 'September';
      case 9: return 'Oktober';
      case 10: return 'November';
      case 11: return 'Dezember';
      default: return ''
    }
  }

}
