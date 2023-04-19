using Common.DAL;
using Common.Req.Course;
using Common.Rsp.DTO;
using DAL.Models;

namespace DAL
{
    public class CourseRep : GenericRep<WebsiteKhoaHocOnline_V4Context, Course>
    {
        public CourseDTO GetACourse(Guid id)
        {
            var courses = new List<CourseDTO>();
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                Course course;
                if((course = context.Courses.SingleOrDefault(c => c.IdCourse == id)!) != null)
                {
                    List<Chapter> chapters = context.Chapters.Where(ch => ch.IdCourseNavigation!.IdCourse == id).OrderBy(ch => ch.Index).ToList();
                    List<ChapterDTO> chapterDTOs = new List<ChapterDTO>();
                    foreach(var chapter in chapters) {
                        List<LessonDTO> lessonDTOs= new List<LessonDTO>();
                        foreach(var lesson in context.Lessons.Where(l => l.IdChapterNavigation.IdChapter == chapter.IdChapter).OrderBy(l => l.Index).ToList())
                        {
                            lessonDTOs.Add(new LessonDTO
                            {
                                IdLesson = lesson.IdLesson,
                                Title = lesson.Title,
                                Index= lesson.Index,
                                Video=lesson.Video,
                            });
                        }
                        chapterDTOs.Add(new ChapterDTO
                        {
                            IdChapter = chapter.IdChapter,
                            Index = chapter.Index,
                            Lessons = lessonDTOs,
                            Name = chapter.Name,
                        });
                    }
                    return new CourseDTO
                    {
                        IdCourse = id,
                        CourseName = course.CourseName,
                        Chapters = chapterDTOs
                    };
                }
            }
            return null!;
        }

        public List<CourseComponent> GetAllCourse(int offset, int limit, string _title_like)
        {
            var res = new List<CourseComponent>();
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var courses = GetAllCourseByName(_title_like); // Get all course whose name contains _title_like

                var data = courses.OrderBy(x => x.CourseName).Skip(offset).Take(limit).ToList(); // pagination

                foreach(var course in data)
                {
                    var user = context.Users.FirstOrDefault(x => x.IdUser== course.IdUser);

                    res.Add(new CourseComponent
                    {
                        Id = course.IdCourse,
                        Title = course.CourseName!,
                        Thumbnail = course.Thumbnail!,
                        AvatarUser = user!.Avatar,
                        NameUser = user!.Name,
                    });
                }
            }
            
            return res;
        }

        public List<CourseComponent> GetAllCourseByExpert(Guid id)
        {

            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var res = new List<CourseComponent>();

                foreach (var course in All.Where(course => course.IdUser == id).ToList())
                {
                    var user = context.Users.FirstOrDefault(x => x.IdUser == course.IdUser);

                    res.Add(new CourseComponent
                    {
                        Id = course.IdCourse,
                        Title = course.CourseName!,
                        Thumbnail = course.Thumbnail!,
                        AvatarUser = user!.Avatar,
                        NameUser = user!.Name,
                    });
                };

                return res;
            }
            
        }

        public IQueryable<Course> GetAllCourseByName(string? _title_like)
        {
            return All.Where(course => course.CourseName.Contains(_title_like==null?"":_title_like));
        }

        public object GetCourseByID(Guid id)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var course = context.Courses.Where(c => c.IdCourse== id).FirstOrDefault();
                if (course == null)
                {
                    return null;
                }
                var user = context.Users.Where(u => u.IdUser == course.IdUser).FirstOrDefault();

                return new
                {
                    Title = course.CourseName,
                    Discount = course.Discount,
                    Price = course.Price,
                    Author = user?.Name,
                    VideoPreview = course.VideoPreview,
                };

            }
        }
    }
}
