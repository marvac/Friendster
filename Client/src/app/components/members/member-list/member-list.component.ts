import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { User } from 'src/app/models/user';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/models/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList = [{ value: 'male', display: 'Men' }, { value: 'female', display: 'Women' }];
  userParameters: any = {};
  pagination: Pagination;

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });

    this.resetFilters(false);
  }

  resetFilters(loadUsers: boolean = true) {
    this.userParameters.gender = this.user.gender === 'male' ? 'female' : 'male';
    this.userParameters.minAge = 18;
    this.userParameters.maxAge = 99;
    this.userParameters.orderBy = 'lastactive';
    if (loadUsers) {
      this.loadUsers();
    }
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParameters)
      .subscribe((result: PaginatedResult<User[]>) => {
      this.users = result.result;
      this.pagination = result.pagination;

    }, error => {
      this.alertify.error(error);
    });
  }
}
