using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ArkaChart.Domain.Batch;
using ArkaChart.Domain.Factory;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Filters;
using ArkaChart.Models;
using ArkaChart.Resources;
using ArkaChart.Tools;

namespace ArkaChart.Controllers {
    public class BatchController : BaseController {
        public ActionResult Index() {
            try {
                CheckDirectory();
                List<FileInfo> receivedFiles = PathHelper.AvalaibleFiles();
                var model = new FilesViewModel(receivedFiles, Repositories.Files.FindAll());
                return View(model);
            } catch (FunctionalException e) {
                AddMessage(ERROR, e.Message);
            }
            return View();
        }

        private void AddMessage(string type , string message) {
            TempData["Message"] = message;
            TempData["MessageClass"] = type;
        }

        public ActionResult Pause(int idProcess) {
            try {
                CheckId(idProcess);
                DataFile dataFile = Repositories.Files.FindDistinctBy(x => x.Id == idProcess);
                if (dataFile != null) {
                    BatchHelper.PauseProcessing(dataFile);
                    AddMessage(WARNING, Batch.Success_Pause);
                }
            } catch (FunctionalException e) {
                AddMessage(ERROR, e.Message);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Launch(string filePath) {
            try {
                CheckFilePath(filePath);
                var dataFile = new DataFile(filePath, (int) StatusHelper.Processing);
                Repositories.Files.Add(dataFile);
                Repositories.SaveChanges();
                BatchHelper.StartProcessing(dataFile);
                AddMessage(SUCCESS, Batch.Success_Launch);
            } catch (FunctionalException e) {
                AddMessage(ERROR, e.Message);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Continue(int idProcess) {
            try {
                CheckId(idProcess);
                DataFile dataFile = Repositories.Files.FindDistinctBy(x => x.Id == idProcess);
                if (dataFile != null) {
                    BatchHelper.StartProcessing(dataFile);
                    AddMessage(SUCCESS, Batch.Success_Continue);
                }
            } catch (FunctionalException e) {
                AddMessage(ERROR, e.Message);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Reset() {
            foreach (var dataFile in Repositories.Files.FindAll()) {
                Repositories.Files.Remove(dataFile);
            }
            return RedirectToAction("Index");
        }

        private void CheckFilePath(string path) {
            if (string.IsNullOrEmpty(path)) {
                throw new FunctionalException(Messages.Error_FileIsEmpty);
            }
            if (!PathHelper.IsExist(path)) {
                throw new FunctionalException(Messages.Error_FileNotExist);
            }
        }

        private void CheckId(int id) {
            if (id == 0) {
                throw new FunctionalException(Messages.Error_ParametersNotCorrect);
            }
        }
        private void CheckDirectory() {
            if (!PathHelper.IsExistReceivingDirectory()) {
                throw new FunctionalException(String.Format( Messages.Error_MustCreateReceivingDirectory, PathHelper.RECEIVING_FOLDER));
            }
        }
    }
}