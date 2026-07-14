import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../../core/services/task.service';
import { CategoryService } from '../../../core/services/category.service';
import { Category } from '../../../core/models/category.model';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-form.html',
  styleUrl: './task-form.scss',
})
export class TaskFormComponent implements OnInit {
  taskId: number | null = null; // null = режим створення, число = режим редагування
  isEditMode = false;

  title = '';
  description = '';
  dueDate = '';
  categoryId: number | null = null;
  isCompleted = false;

  categories: Category[] = [];
  errorMessage = '';
  isLoading = false;

  constructor(
    private taskService: TaskService,
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.loadCategories();

    // Перевіряємо, чи є параметр "id" в URL - визначає режим
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.taskId = Number(idParam);
      this.isEditMode = true;
      this.loadTask(this.taskId);
    }
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (categories) => (this.categories = categories),
      error: (err) => console.error(err),
    });
  }

  loadTask(id: number): void {
    this.isLoading = true;
    this.taskService.getTaskById(id).subscribe({
      next: (task) => {
        this.title = task.title;
        this.description = task.description || '';
        this.dueDate = task.dueDate ? task.dueDate.substring(0, 10) : ''; // формат yyyy-MM-dd для <input type="date">
        this.categoryId = task.categoryId;
        this.isCompleted = task.isCompleted;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Не вдалось завантажити задачу';
        this.isLoading = false;
        console.error(err);
      },
    });
  }

  onSubmit(): void {
    this.errorMessage = '';

    if (this.isEditMode && this.taskId) {
      this.taskService
        .updateTask(this.taskId, {
          title: this.title,
          description: this.description || null,
          isCompleted: this.isCompleted,
          dueDate: this.dueDate || null,
          categoryId: this.categoryId,
        })
        .subscribe({
          next: () => this.router.navigate(['/tasks']),
          error: (err) => {
            this.errorMessage = 'Не вдалось оновити задачу';
            console.error(err);
          },
        });
    } else {
      this.taskService
        .createTask({
          title: this.title,
          description: this.description || null,
          dueDate: this.dueDate || null,
          categoryId: this.categoryId,
        })
        .subscribe({
          next: () => this.router.navigate(['/tasks']),
          error: (err) => {
            this.errorMessage = 'Не вдалось створити задачу';
            console.error(err);
          },
        });
    }
  }

  onCancel(): void {
    this.router.navigate(['/tasks']);
  }
}
