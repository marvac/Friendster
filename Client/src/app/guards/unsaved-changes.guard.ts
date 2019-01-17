import { Injectable } from '@angular/core';
import { MemberEditComponent } from '../components/members/member-edit/member-edit.component';
import { CanDeactivate } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {

  constructor() { }

  canDeactivate(component: MemberEditComponent) {
    if (component.editForm.dirty) {
      return confirm("Are you sure you want to navigate away with unsaved changes?");
    }
    return true;
  }
}
