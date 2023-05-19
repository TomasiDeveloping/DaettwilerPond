import {Component} from '@angular/core';
import {environment} from "../../environments/environment";

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {
  public isUserLoggedIn: boolean = false;
  public isShown: boolean = false;

  public version: string = environment.version;

}
