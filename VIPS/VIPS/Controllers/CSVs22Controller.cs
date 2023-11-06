using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VIPS.Data;
using VIPS.Models;

namespace VIPS.Controllers
{
    public class CSVs22Controller : Controller
    {
        private readonly ApplicationDbContex _context;

        public CSVs22Controller(ApplicationDbContex context)
        {
            _context = context;
        }

        // GET: CSVs22
        public async Task<IActionResult> Index()
        {
              return _context.CSVs != null ? 
                          View(await _context.CSVs.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContex.CSVs'  is null.");
        }

        // GET: CSVs22/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CSVs == null)
            {
                return NotFound();
            }

            var cSV = await _context.CSVs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cSV == null)
            {
                return NotFound();
            }

            return View(cSV);
        }

        // GET: CSVs22/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CSVs22/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContractID,CreatedOn,CreatedBy,ContractName,ContractOrigin,ContractTypeName,CurrentStageAssignees,DaysInCurrStage,Description,ExternalContractReferenceID,FolderName,Locked,Owner,PrimaryDocument,RelatedToContract,RelatedToContractID,StageName,UpdatedBy,UpdatedOn,Workflow,ProgramsOrCourses,CCECMajors,AutoRenewal,ContractCategory,AgencyMailingAddress1,AgencyMailingAddress2,AgencyName,BCH_AgingServicesManagement,BCH_AthleticTraining,BCH_College,BCH_ExerciseScience,BCH_HealthAdministration,BCH_InterdisciplinaryHealthStudies,BCH_MentalHealthCounseling,BCH_NurseAnesthetist,BCH_Nursing,BCH_NutritionDietetics,BCH_PhysicalTherapy,BCH_PublicHealth,City,COEHSPrograms,Department,EmailAddress,FacultyInitiator,Graduate_Undergraduate,PhoneNumber,PrimaryContact,Renewal,State,TitleCert,Year,ZipCode,Error,ErrorDescription,Duplicate")] CSV cSV)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cSV);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cSV);
        }

        // GET: CSVs22/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CSVs == null)
            {
                return NotFound();
            }

            var cSV = await _context.CSVs.FindAsync(id);
            if (cSV == null)
            {
                return NotFound();
            }
            return View(cSV);
        }

        // POST: CSVs22/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContractID,CreatedOn,CreatedBy,ContractName,ContractOrigin,ContractTypeName,CurrentStageAssignees,DaysInCurrStage,Description,ExternalContractReferenceID,FolderName,Locked,Owner,PrimaryDocument,RelatedToContract,RelatedToContractID,StageName,UpdatedBy,UpdatedOn,Workflow,ProgramsOrCourses,CCECMajors,AutoRenewal,ContractCategory,AgencyMailingAddress1,AgencyMailingAddress2,AgencyName,BCH_AgingServicesManagement,BCH_AthleticTraining,BCH_College,BCH_ExerciseScience,BCH_HealthAdministration,BCH_InterdisciplinaryHealthStudies,BCH_MentalHealthCounseling,BCH_NurseAnesthetist,BCH_Nursing,BCH_NutritionDietetics,BCH_PhysicalTherapy,BCH_PublicHealth,City,COEHSPrograms,Department,EmailAddress,FacultyInitiator,Graduate_Undergraduate,PhoneNumber,PrimaryContact,Renewal,State,TitleCert,Year,ZipCode,Error,ErrorDescription,Duplicate")] CSV cSV)
        {
            if (id != cSV.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cSV);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CSVExists(cSV.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cSV);
        }

        // GET: CSVs22/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CSVs == null)
            {
                return NotFound();
            }

            var cSV = await _context.CSVs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cSV == null)
            {
                return NotFound();
            }

            return View(cSV);
        }

        // POST: CSVs22/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CSVs == null)
            {
                return Problem("Entity set 'ApplicationDbContex.CSVs'  is null.");
            }
            var cSV = await _context.CSVs.FindAsync(id);
            if (cSV != null)
            {
                _context.CSVs.Remove(cSV);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CSVExists(int id)
        {
          return (_context.CSVs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
