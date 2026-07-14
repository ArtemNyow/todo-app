import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { TaskService } from '../../../core/services/task.service';
import { AuthService } from '../../../core/services/auth.service';
import { Task, TaskQueryParams } from '../../../core/models/task.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './task-list.html',
  styleUrl: './task-list.scss',
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  isLoading = false;
  errorMessage = '';

  constructor(
    private taskService: TaskService,
    private authService: AuthService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const params: TaskQueryParams = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
    };

    this.taskService.getTasks(params).subscribe({
      next: (result) => {
        this.tasks = result.items;
        this.totalCount = result.totalCount;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Не вдалось завантажити задачі';
        this.isLoading = false;
        console.error(err);
      },
    });
  }

  onDelete(id: number, event: Event): void {
    event.stopPropagation();
    if (!confirm('Видалити цю задачу?')) return;

    this.taskService.deleteTask(id).subscribe({
      next: () => {
        this.loadTasks();
      },
      error: (err) => {
        this.errorMessage = 'Не вдалось видалити задачу';
        console.error(err);
      },
    });
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
