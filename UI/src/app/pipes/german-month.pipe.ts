import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'germanMonth',
  standalone: true
})
export class GermanMonthPipe implements PipeTransform {

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