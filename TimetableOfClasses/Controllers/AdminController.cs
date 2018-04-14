using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using TimetableOfClasses.Models;
using TimetableOfClasses.Models.ViewModels;

namespace TimetableOfClasses.Controllers
{
    [Authorize(Roles = AppIdentityDbContext.ADMIN)]
    public class AdminController : Controller
    {
        private IUserRepository userRepository;
        private IDisciplineRepository disciplineRepository;
        private IGroupRepository groupRepository;
        private ITeacherRepository teacherRepository;
        private ITimetableRepository timetableRepository;

        public AdminController(IUserRepository userRepository, IDisciplineRepository disciplineRepository,
            IGroupRepository groupRepository, ITeacherRepository teacherRepository,
            ITimetableRepository timetableRepository)
        {
            this.userRepository = userRepository;
            this.disciplineRepository = disciplineRepository;
            this.groupRepository = groupRepository;
            this.teacherRepository = teacherRepository;
            this.timetableRepository = timetableRepository;
        }

        public ViewResult Index() => View();

        public IActionResult Error()
        {
            string message = (string)TempData["message"];
            if (string.IsNullOrEmpty(message))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(new ErrorViewModel { Message = message });
            }
        }

        #region Manager Users

        private SelectList GetAllRoles() => new SelectList(new string[] { AppIdentityDbContext.STUDENT, AppIdentityDbContext.ADMIN });

        public ViewResult Users() => View(userRepository.Users.ToList());

        [HttpGet]
        public async Task<ViewResult> EditUser(string userId)
        {
            var user = userRepository.Users.FirstOrDefault(u => u.Id == userId);
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                SelectedRole = await userRepository.GetRoleUser(user),
                Roles = GetAllRoles()
            };
            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult EditUser(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                userRepository.SaveUser(user);
                TempData["message"] = $"{user.UserName} has been saved";
                return RedirectToAction(nameof(Users));
            }
            else
            {
                return View(user);
            }
        }

        public ViewResult CreateUser() => View(nameof(EditUser), new UserViewModel { Roles = GetAllRoles() });

        [HttpGet]
        public IActionResult ChangePassword(string userId)
        {
            var user = userRepository.Users.FirstOrDefault(u => u.Id == userId);
            return View(new ChangePasswordViewModel { Id = user.Id, UserName = user.UserName });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            IdentityUser user = await userRepository.ChangePasswordAsync(changePasswordViewModel);
            if (user != null)
            {
                TempData["message"] = $"Password for {user.UserName} was changed";
                return RedirectToAction(nameof(Users));
            }
            else
            {
                TempData["message"] = $"Old password wrong!";
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            IdentityUser deletedUser = await userRepository.DeleteUserAsync(userId);
            if (deletedUser != null)
            {
                TempData["message"] = $"{deletedUser.UserName} was deleted";
            }
            return RedirectToAction(nameof(Users));
        }

        #endregion

        #region Manager Disciplines

        public ViewResult Disciplines() => View(disciplineRepository.Disciplines);

        [HttpGet]
        public ViewResult EditDiscipline(int disciplineId) =>
            View(disciplineRepository.Disciplines.FirstOrDefault(d => d.DisciplineId == disciplineId));

        [HttpPost]
        public IActionResult EditDiscipline(Discipline discipline)
        {
            if (ModelState.IsValid)
            {
                disciplineRepository.SaveDiscipline(discipline);
                TempData["message"] = $"{discipline.Name} has been saved";
                return RedirectToAction(nameof(Disciplines));
            }
            else
            {
                return View(discipline);
            }
        }

        public ViewResult CreateDiscipline() => View(nameof(EditDiscipline), new Discipline());

        [HttpPost]
        public IActionResult DeleteDiscipline(int disciplineId)
        {
            try
            {
                Discipline deletedDiscipline = disciplineRepository.DeleteDiscipline(disciplineId);
                if (deletedDiscipline != null)
                {
                    TempData["message"] = $"{deletedDiscipline.Name} was deleted";
                }
                return RedirectToAction(nameof(Disciplines));
            }
            catch (Exception e)
            {
                TempData["message"] = e.Message;
                return RedirectToAction(nameof(Error));
            }
        }

        #endregion

        #region Manager Groups

        public ViewResult Groups() => View(groupRepository.Groups);

        [HttpGet]
        public ViewResult EditGroup(int groupId) =>
            View(groupRepository.Groups.FirstOrDefault(g => g.GroupId == groupId));

        [HttpPost]
        public IActionResult EditGroup(Group group)
        {
            if (ModelState.IsValid)
            {
                groupRepository.SaveGroup(group);
                TempData["message"] = $"{group.Name} has been saved";
                return RedirectToAction(nameof(Groups));
            }
            else
            {
                return View(group);
            }
        }

        public ViewResult CreateGroup() => View(nameof(EditGroup), new Group());

        [HttpPost]
        public IActionResult DeleteGroup(int groupId)
        {
            try
            {
                Group deletedGroup = groupRepository.DeleteGroup(groupId);
                if (deletedGroup != null)
                {
                    TempData["message"] = $"{deletedGroup.Name} was deleted";
                }
                return RedirectToAction(nameof(Groups));
            }
            catch (Exception e)
            {
                TempData["message"] = e.Message;
                return RedirectToAction(nameof(Error));
            }
        }

        #endregion

        #region Manager Teacher

        public ViewResult Teachers() => View(teacherRepository.Teachers);

        [HttpGet]
        public ViewResult EditTeacher(int teacherId)
        {
            var teacher = teacherRepository.Teachers.FirstOrDefault(t => t.TeacherId == teacherId);

            var disciplineChecked = disciplineRepository.Disciplines
                .Select(d => new DisciplineCheckedViewModel { DisciplineId = d.DisciplineId })
                .ToList();
            // set values to disciplineChecked
            disciplineChecked.Where(d => teacher.TeacherDisciplines.Where(td => td.TeacherId == teacherId)
                .Select(c => c.DisciplineId).Contains(d.DisciplineId))
                .ToList()
                .ForEach(n => n.Checked = true);

            var teacherViewModel = new TeacherViewModel
            {
                Teacher = teacher,
                AllDiscipline = disciplineRepository.Disciplines.OrderBy(d => d.DisciplineId).ToList(),
                DisciplineChecked = disciplineChecked.OrderBy(d => d.DisciplineId).ToList()
            };

            return View(teacherViewModel);
        }

        [HttpPost]
        public IActionResult EditTeacher(TeacherViewModel teacherViewModel)
        {
            if (ModelState.IsValid)
            {
                int[] disciplineChecked = teacherViewModel.DisciplineChecked.Where(a => a.Checked)
                    .Select(n => n.DisciplineId).ToArray();
                var disciplines = disciplineRepository.Disciplines
                    .Where(d => disciplineChecked.Contains(d.DisciplineId))
                    .ToList();

                teacherRepository.SaveTeacher(teacherViewModel.Teacher, disciplines);
                TempData["message"] = $"{teacherViewModel.Teacher.Name} has been saved";
                return RedirectToAction(nameof(Teachers));
            }
            else
            {
                return View(teacherViewModel.Teacher);
            }
        }

        public ViewResult CreateTeacher()
        {
            var allDiscipline = disciplineRepository.Disciplines.OrderBy(d => d.DisciplineId).ToList();
            var disciplineChecked = allDiscipline
                .Select(d => new DisciplineCheckedViewModel { DisciplineId = d.DisciplineId })
                .OrderBy(d => d.DisciplineId)
                .ToList();

            var teacherViewModel = new TeacherViewModel
            {
                Teacher = new Teacher(),
                AllDiscipline = allDiscipline,
                DisciplineChecked = disciplineChecked
            };

            return View(nameof(EditTeacher), teacherViewModel);
        }

        [HttpPost]
        public IActionResult DeleteTeacher(int teacherId)
        {
            try
            {
                Teacher deletedTeacher = teacherRepository.DeleteTeacher(teacherId);
                if (deletedTeacher != null)
                {
                    TempData["message"] = $"{deletedTeacher.Name} was deleted";
                }
                return RedirectToAction(nameof(Teachers));
            }
            catch (Exception e)
            {
                TempData["message"] = e.Message;
                return RedirectToAction(nameof(Error));
            }

        }

        #endregion

        #region Manager Timetable

        public ViewResult Timetables() => View(timetableRepository.Timetables);

        [HttpGet]
        public ViewResult EditTimetable(int timetableId)
        {
            var timetable = timetableRepository.Timetables.FirstOrDefault(t => t.TimetableId == timetableId);
            var timetableViewModel = new TimetableViewModel
            {
                Timetable = timetable,
                Disciplines = new SelectList(disciplineRepository.Disciplines.Select(d => d.Name).ToList()),
                Teachers = new SelectList(teacherRepository.Teachers.Select(t => t.Name).ToList()),
                Groups = new SelectList(groupRepository.Groups.Select(g => g.Name).ToList()),
                SelectedDiscipline = timetable?.Discipline?.Name,
                SelectedTeacher = timetable?.Teacher?.Name,
                SelectedGroup = timetable?.Group?.Name
            };
            return View(timetableViewModel);
        }

        [HttpPost]
        public IActionResult EditTimetable(TimetableViewModel timetableViewModel)
        {
            if (ModelState.IsValid)
            {
                timetableViewModel.Timetable.Discipline = disciplineRepository.Disciplines
                    .FirstOrDefault(d => d.Name == timetableViewModel.SelectedDiscipline);
                timetableViewModel.Timetable.Teacher = teacherRepository.Teachers
                    .FirstOrDefault(d => d.Name == timetableViewModel.SelectedTeacher);
                timetableViewModel.Timetable.Group = groupRepository.Groups
                   .FirstOrDefault(d => d.Name == timetableViewModel.SelectedGroup);

                timetableRepository.SaveTimetable(timetableViewModel.Timetable);
                TempData["message"] = $"Timetable {timetableViewModel.Timetable.TimetableId} has been saved";
                return RedirectToAction(nameof(Timetables));
            }
            else
            {
                return View(timetableViewModel);
            }
        }

        public ViewResult CreateTimetable()
        {
            var timetableViewModel = new TimetableViewModel
            {
                Timetable = new Timetable(),
                Disciplines = new SelectList(disciplineRepository.Disciplines.Select(d => d.Name).ToList()),
                Teachers = new SelectList(teacherRepository.Teachers.Select(t => t.Name).ToList()),
                Groups = new SelectList(groupRepository.Groups.Select(g => g.Name).ToList())
            };

            return View(nameof(EditTimetable), timetableViewModel);
        }

        [HttpPost]
        public IActionResult DeleteTimetable(int timetableId)
        {
            Timetable deletedTimetable = timetableRepository.DeleteTimetable(timetableId);
            if (deletedTimetable != null)
            {
                TempData["message"] = $"Timetable {deletedTimetable.TimetableId} was deleted";
            }
            return RedirectToAction(nameof(Timetables));
        }

        #endregion
    }
}
